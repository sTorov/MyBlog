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
    const list = view.querySelectorAll('.badge.tag-badge');
    const listTags = Array.from(list).map(item => item.innerText);

    if (listTags.indexOf(value) === -1) {
        if(view.innerText === 'Здесь вы увидите добавленные теги') view.innerText = null;
        createTag(value, view);
        field.value += ` ${value}`;
    }
}

function changeViewTags(el, cont) {
    const input = document.getElementById(el);
    const container = document.getElementById(cont);

    var tags = input.value.split(' ');
    if (tags.length > 0) {
        tags.forEach(item => createTag(item, container));
    }
}

function createTag(text, container) {
    const newItem = document.createElement('div');
    newItem.classList.add('badge', 'tag-badge');
    newItem.innerText = text;
    newItem.addEventListener('click', () => deleteTag(newItem));
    container.insertAdjacentElement('beforeEnd', newItem);
}

function deleteTag(elem) {
    const field = document.getElementById('post-tags');
    const view = document.getElementById('add-tags');

    elem.remove();
    const tags = field.value.split(' ');
    const index = tags.indexOf(elem.innerText);
    tags.splice(index, 1);
    field.value = tags.join(' ');
    if (tags.length === 1 && tags[0] === '') view.innerText = 'Здесь вы увидите добавленные теги';
}


function setScrollPos() {
    localStorage.setItem('y-pos', window.pageYOffset);
}

function getScrollPos() {
    const pos = localStorage.getItem('y-pos');
    if (pos !== null) {
        window.scrollTo({ left: 0, top: pos, behavior: "instant" });
        localStorage.removeItem('y-pos');
    }
}