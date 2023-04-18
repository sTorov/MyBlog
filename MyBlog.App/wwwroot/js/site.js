// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

function getFilterInputValue() {
    return document.getElementById('filter-input').value.toLowerCase();
}

function filter() {
    const items = document.querySelectorAll('#tags option');

    items.forEach(item => {
        const title = item.innerText.toLowerCase();
        if (!title.includes(getFilterInputValue())) {
            item.style.display = 'none';
        }
        else {
            item.style.display = 'block';
        }
    })
}

function addTag() {
    const select = document.getElementById('tags');
    const view = document.getElementById('add-tags');
    const field = document.getElementById('post-tags');

    const index = select.options.selectedIndex;
    if (index == -1) return;

    const value = select.options[index].innerText;
    if (view.innerText.split(' ').indexOf(value) === -1) {
        view.innerText = view.innerText == 'Здесь вы увидите добавленные теги'
            ? `${value}`
            : view.innerText + ` ${value}`;
        field.value += ` ${value}`;
    }
}