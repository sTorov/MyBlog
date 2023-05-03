/** Получение значения инпута фильтрации тегов */
function getFilterInputValue() {
    return document.getElementById('filter-input').value.toLowerCase();
}

/** Фильтрация тегов */
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

/** Добавление элемента тега */
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

/** Отображение добавляемых тегов к статье */
function changeViewTags(el, cont) {
    const input = document.getElementById(el);
    const container = document.getElementById(cont);

    var tags = input.value.split(' ');
    if (tags.length > 0) {
        tags.forEach(item => createTag(item, container));
    }
}

/** Создание нового элемента тега, добавление его в контейнер */
function createTag(text, container) {
    const newItem = document.createElement('div');
    newItem.classList.add('badge', 'tag-badge');
    newItem.innerText = text;
    newItem.addEventListener('click', () => deleteTag(newItem));
    container.insertAdjacentElement('beforeEnd', newItem);
}

/** Удаление элемента тега */
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

/** Сохранение положения прокрутки страницы в локальном хранилище */
function setScrollPos() {
    localStorage.setItem('y-pos', window.pageYOffset);
}

/** Прокрутка страницы до указанного значения в локальном хранилище */
function getScrollPos() {
    const pos = localStorage.getItem('y-pos');
    if (pos !== null) {
        window.scrollTo({ left: 0, top: pos, behavior: "instant" });
        localStorage.removeItem('y-pos');
    }
}

/** Сброс значения поля даты */
function resetDataInDateInput() {
    const input = document.querySelector('input[type=date]');
    input.value = '';
}