const container = document.querySelector('.overtime-entries');
const draggables = document.querySelectorAll('.overtime-entry');

let draggedElement = null;
let originalIndex = -1;

draggables.forEach((draggable, index) => {
    draggable.draggable = true;

    draggable.addEventListener('dragstart', (e) => {
        draggedElement = draggable;
        originalIndex = index;
        e.dataTransfer.setData('text/plain', ''); // Required for Firefox
        setTimeout(() => {
            draggable.classList.add('dragging');
        }, 0);
    });

    draggable.addEventListener('dragend', () => {
        draggedElement = null;
        originalIndex = -1;
        draggable.classList.remove('dragging');
    });
});

container.addEventListener('dragover', (e) => {
    e.preventDefault();

    if (!draggedElement) return;

    const newIndex = [...container.querySelectorAll('.overtime-entry')].indexOf(draggedElement);
    if (newIndex !== originalIndex) {
        if (newIndex < originalIndex) {
            container.insertBefore(draggedElement, draggables[newIndex]);
        } else {
            container.insertBefore(draggedElement, draggables[newIndex + 1]);
        }
        originalIndex = newIndex;
    }
});

