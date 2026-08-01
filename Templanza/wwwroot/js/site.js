// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

document.addEventListener('DOMContentLoaded', function () {
    mostrarToastsDeTempData();
    inicializarConfirmaciones();
    inicializarSteppers();
    inicializarAnimacionesDeEntrada();
});

// Muestra un toast de SweetAlert2 con los mensajes que la app deja en TempData["Exito"] / TempData["Error"].
function mostrarToastsDeTempData() {
    var datos = document.getElementById('toast-data');
    if (!datos || typeof Swal === 'undefined') return;

    var exito = datos.dataset.exito;
    var error = datos.dataset.error;

    if (exito) {
        Swal.fire({
            toast: true,
            position: 'top-end',
            icon: 'success',
            title: exito,
            showConfirmButton: false,
            timer: 3000,
            timerProgressBar: true
        });
    }

    if (error) {
        Swal.fire({
            toast: true,
            position: 'top-end',
            icon: 'error',
            title: error,
            showConfirmButton: false,
            timer: 4000,
            timerProgressBar: true
        });
    }
}

// Cualquier <form data-confirm="mensaje"> pide confirmación con SweetAlert2 antes de enviarse.
function inicializarConfirmaciones() {
    if (typeof Swal === 'undefined') return;

    document.querySelectorAll('form[data-confirm]').forEach(function (form) {
        form.addEventListener('submit', function (evento) {
            if (form.dataset.confirmado === 'true') return;

            evento.preventDefault();
            Swal.fire({
                title: form.dataset.confirm || '¿Confirmás esta acción?',
                icon: 'warning',
                showCancelButton: true,
                confirmButtonText: 'Sí, confirmar',
                cancelButtonText: 'Cancelar',
                confirmButtonColor: '#4a7c59'
            }).then(function (resultado) {
                if (resultado.isConfirmed) {
                    form.dataset.confirmado = 'true';
                    form.submit();
                }
            });
        });
    });
}

// Botones +/- junto a un <input type="number"> con la clase .stepper-cantidad-input.
function inicializarSteppers() {
    document.querySelectorAll('.stepper-cantidad').forEach(function (stepper) {
        var input = stepper.querySelector('.stepper-cantidad-input');
        if (!input) return;

        var min = parseInt(input.min || '1', 10);
        var max = input.max ? parseInt(input.max, 10) : null;

        stepper.querySelectorAll('[data-step]').forEach(function (boton) {
            boton.addEventListener('click', function () {
                var valor = parseInt(input.value || min, 10) || min;
                valor += parseInt(boton.dataset.step, 10);
                if (valor < min) valor = min;
                if (max !== null && valor > max) valor = max;
                input.value = valor;
            });
        });
    });
}

// Hace aparecer con un fade-in-up las tarjetas marcadas con esa clase al entrar en pantalla.
function inicializarAnimacionesDeEntrada() {
    var elementos = document.querySelectorAll('.fade-in-up');
    if (!elementos.length) return;

    if (typeof IntersectionObserver === 'undefined') {
        elementos.forEach(function (el) { el.classList.add('visible'); });
        return;
    }

    var observer = new IntersectionObserver(function (entradas) {
        entradas.forEach(function (entrada) {
            if (entrada.isIntersecting) {
                entrada.target.classList.add('visible');
                observer.unobserve(entrada.target);
            }
        });
    }, { threshold: 0.1 });

    elementos.forEach(function (el) { observer.observe(el); });
}
