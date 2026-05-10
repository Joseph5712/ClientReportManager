// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
\
// Cierra automáticamente los mensajes de éxito después de unos segundos.
// Esto mantiene la pantalla limpia sin quitar retroalimentación al usuario.
document.addEventListener("DOMContentLoaded", function () {
    const alerts = document.querySelectorAll(".alert.alert-success");

    alerts.forEach(function (alert) {
        setTimeout(function () {
            const bootstrapAlert = bootstrap.Alert.getOrCreateInstance(alert);
            bootstrapAlert.close();
        }, 4000);
    });
});
