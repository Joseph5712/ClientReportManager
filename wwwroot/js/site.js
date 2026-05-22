// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

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

// Manejo de Modo Oscuro (Tema)
document.addEventListener("DOMContentLoaded", function () {
    const toggleButton = document.getElementById("theme-toggle");
    const toggleIcon = document.getElementById("theme-toggle-icon");
    
    if (toggleButton && toggleIcon) {
        const updateIcon = (theme) => {
            toggleIcon.textContent = theme === 'dark' ? '☀️' : '🌙';
        };

        const currentTheme = document.documentElement.getAttribute('data-theme') || 'light';
        updateIcon(currentTheme);

        toggleButton.addEventListener("click", function () {
            const currentTheme = document.documentElement.getAttribute('data-theme') || 'light';
            const newTheme = currentTheme === 'dark' ? 'light' : 'dark';
            
            document.documentElement.setAttribute('data-theme', newTheme);
            localStorage.setItem('theme', newTheme);
            updateIcon(newTheme);
        });
    }
});
