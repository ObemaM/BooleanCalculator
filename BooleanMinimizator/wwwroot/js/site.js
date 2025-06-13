﻿let toggleButton = document.getElementById("toggleKeyboard");
let keyboard = document.getElementById("keyboard");

// Проверяем состояние из localStorage при загрузке
if (localStorage.getItem("keyboardVisible") === "false") {
    keyboard.style.display = "none";
    toggleButton.innerText = "Показать клавиатуру";
} else {
    keyboard.style.display = "grid";
    toggleButton.innerText = "Скрыть клавиатуру";
}

toggleButton.addEventListener("click", function () {
    if (keyboard.style.display === "none") {
        keyboard.style.display = "grid";
        toggleButton.innerText = "Скрыть клавиатуру";
        localStorage.setItem("keyboardVisible", "true");
    } else {
        keyboard.style.display = "none";
        toggleButton.innerText = "Показать клавиатуру";
        localStorage.setItem("keyboardVisible", "false");
    }
});

// Логика ввода символов с клавиатуры
document.querySelectorAll(".key").forEach(button => {
    button.addEventListener("click", function () {
        const input = document.getElementById("inputField");
        const start = input.selectionStart;
        const end = input.selectionEnd;
        const text = this.innerText;

        // Проверка: не вставлять, если длина уже 64 символа
        if (input.value.length - (end - start) + text.length > 64) {
            // Можно добавить короткое уведомление, если нужно
            return;
        }

        // Вставка текста в позицию курсора
        input.value = input.value.slice(0, start) + text + input.value.slice(end);

        // Перемещение курсора после вставленного текста
        const newPos = start + text.length;
        input.setSelectionRange(newPos, newPos);
        input.focus(); // Сохраняем фокус
    });
});

// Подсказка - теперь кнопка поддержки является прямой ссылкой HTML
