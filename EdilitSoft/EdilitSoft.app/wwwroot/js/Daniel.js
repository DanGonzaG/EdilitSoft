/*const transporte = parseFloat( document.getElementById('transporte').textContent);
const otrosCostos = parseFloat(document.getElementById('otrosCostos').value);
const ganacia = parseFloat(document.getElementById('ganancia').value);
const total = parseFloat(transporte + otrosCostos + ganacia);

console.log(transporte)*/






let valor1 = 0;
let valor2 = 0;
let valor3 = 0;


const campoTrans = document.getElementById('transporte');
campoTrans.addEventListener('input', function (even) {
    const num = even.target.value;
    if (isNaN(num) || num == '') {
        valor1 = 0;
        console.log(valor1)
    }
    else {
        valor1 = parseFloat(num);
        console.log(valor1)
    }
    mostrarTotal();
});

const campoOtrosC = document.getElementById('otrosCostos');
campoOtrosC.addEventListener('input', function (even) {
    const num = even.target.value;
    if (isNaN(num) || num == '') {
        valor2 = 0;
        console.log(valor2)
    }
    else {
        valor2 = parseFloat(num);
        console.log(valor2)
    }
    mostrarTotal();
});

const ganancia = document.getElementById('ganancia');
ganancia.addEventListener('input', function (even) {
    const num = even.target.value;
    if (isNaN(num) || num == '') {
        valor3 = 0;
        console.log(valor3)
    }
    else {
        valor3 = parseFloat(num);
        console.log(valor3)
    }
    mostrarTotal();
});

const campoTotal = document.getElementById('total')
function mostrarTotal() {
    const total = valor1 + valor2 + valor3;
    console.log('Total:', total);
    campoTotal.value = total;    
}







