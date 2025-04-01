using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EdilitSoft.app.Migrations
{
    /// <inheritdoc />
    public partial class MOV_ModificacionRelacionesConCategoria : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                BEGIN TRANSACTION;
                
                -- Paso 1: Identificar y deshabilitar TODAS las restricciones FK
                DECLARE @sql NVARCHAR(MAX) = '';
                
                -- Generar comandos para deshabilitar constraints
                SELECT @sql = @sql + 'ALTER TABLE ' + OBJECT_NAME(parent_object_id) + 
                              ' NOCHECK CONSTRAINT ' + name + ';'
                FROM sys.foreign_keys
                WHERE referenced_object_id = OBJECT_ID('Inventario');
                
                EXEC sp_executesql @sql;
                
                -- Paso 2: Crear nueva tabla con IDENTITY
                CREATE TABLE [dbo].[Inventario_new](
                    [IdArticulo] [int] IDENTITY(1,1) NOT NULL,
                    [IdLibro] [int] NOT NULL,
                    [Fecha] [datetime2] NOT NULL,
                    [Existencias] [int] NOT NULL,
                    [Precio] [decimal](18,2) NOT NULL,
                    [Activo] [bit] NOT NULL,
                    CONSTRAINT [PK_Inventario_new] PRIMARY KEY ([IdArticulo])
                );
                
                -- Paso 3: Copiar datos preservando IDs
                SET IDENTITY_INSERT [dbo].[Inventario_new] ON;
                
                INSERT INTO [dbo].[Inventario_new]
                    ([IdArticulo], [IdLibro], [Fecha], [Existencias], [Precio], [Activo])
                SELECT
                    [IdArticulo], [IdLibro], [Fecha], [Existencias], [Precio], [Activo]
                FROM [dbo].[Inventario];
                
                SET IDENTITY_INSERT [dbo].[Inventario_new] OFF;
                
                -- Paso 4: Renombrar tablas (evitando DROP directo)
                EXEC sp_rename 'Inventario', 'Inventario_old';
                EXEC sp_rename 'Inventario_new', 'Inventario';
                
                -- Paso 5: Actualizar restricciones FK en tablas dependientes
                -- Primero eliminar las viejas
                SET @sql = '';
                SELECT @sql = @sql + 'ALTER TABLE ' + OBJECT_NAME(parent_object_id) + 
                              ' DROP CONSTRAINT ' + name + ';'
                FROM sys.foreign_keys
                WHERE referenced_object_id = OBJECT_ID('Inventario_old');
                
                EXEC sp_executesql @sql;
                
                -- Luego crear las nuevas
                -- Ejemplo para Catalogo:
                ALTER TABLE [dbo].[Catalogo] ADD CONSTRAINT [FK_Catalogo_Inventario_IdArticuloFK]
                    FOREIGN KEY([IdArticuloFK]) REFERENCES [dbo].[Inventario] ([IdArticulo]);
                
                -- Paso 6: Habilitar verificación de constraints
                SET @sql = '';
                SELECT @sql = @sql + 'ALTER TABLE ' + OBJECT_NAME(parent_object_id) + 
                              ' WITH CHECK CHECK CONSTRAINT ' + name + ';'
                FROM sys.foreign_keys
                WHERE referenced_object_id = OBJECT_ID('Inventario');
                
                EXEC sp_executesql @sql;
                
                -- Paso 7: Eliminar tabla vieja (opcional, se puede hacer después)
                -- DROP TABLE [dbo].[Inventario_old];
                
                COMMIT TRANSACTION;
            ");
        
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                -- Implementación de rollback (similar pero inversa)
                ALTER TABLE [dbo].[Catalogo] NOCHECK CONSTRAINT [FK_Catalogo_Inventario_IdArticuloFK];
                
                CREATE TABLE [dbo].[Inventario_old](
                    [IdArticulo] [int] NOT NULL,
                    [IdLibro] [int] NOT NULL,
                    [Fecha] [datetime2] NOT NULL,
                    [Existencias] [int] NOT NULL,
                    [Precio] [decimal](18,2) NOT NULL,
                    [Activo] [bit] NOT NULL,
                    CONSTRAINT [PK_Inventario_old] PRIMARY KEY ([IdArticulo])
                );
                
                INSERT INTO [dbo].[Inventario_old]
                    ([IdArticulo], [IdLibro], [Fecha], [Existencias], [Precio], [Activo])
                SELECT
                    [IdArticulo], [IdLibro], [Fecha], [Existencias], [Precio], [Activo]
                FROM [dbo].[Inventario];
                
                DROP TABLE [dbo].[Inventario];
                
                EXEC sp_rename 'Inventario_old', 'Inventario';
                
                ALTER TABLE [dbo].[Catalogo] WITH CHECK ADD CONSTRAINT [FK_Catalogo_Inventario_IdArticuloFK]
                    FOREIGN KEY([IdArticuloFK]) REFERENCES [dbo].[Inventario] ([IdArticulo]);
                
                ALTER TABLE [dbo].[Catalogo] CHECK CONSTRAINT [FK_Catalogo_Inventario_IdArticuloFK];
            ");
        }
    }
}

