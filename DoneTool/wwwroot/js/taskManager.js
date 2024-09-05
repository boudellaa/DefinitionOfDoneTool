document.addEventListener('DOMContentLoaded', function () {
    const dropdowns = document.querySelectorAll('.custom-dropdown');
    const textareas = document.querySelectorAll('textarea.form-control');

    if (dropdowns.length > 0) {
        initializeDropdowns(dropdowns);
    } else {
        console.warn('No dropdowns found on the page.');
    }

    if (textareas.length > 0) {
        initializeTextareas(textareas);
    } else {
        console.warn('No textareas found on the page.');
    }

    function initializeDropdowns(dropdowns) {
        dropdowns.forEach(function (dropdown) {
            const dropdownToggle = dropdown.querySelector('.dropdown-toggle');
            const dropdownMenu = dropdown.querySelector('.dropdown-menu');
            const hiddenInput = dropdown.querySelector('input[type="hidden"]');

            if (!dropdownToggle || !dropdownMenu || !hiddenInput) {
                console.warn('Dropdown elements are missing in the DOM.');
                return;
            }

            const initialValue = hiddenInput.value;
            const initialItem = dropdownMenu.querySelector(`li[data-value="${initialValue}"]`);

            if (initialItem) {
                dropdownToggle.textContent = initialItem.textContent;
                dropdownToggle.className = 'dropdown-toggle ' + initialItem.className;
            }

            dropdownToggle.addEventListener('click', function () {
                dropdownMenu.style.display = dropdownMenu.style.display === 'block' ? 'none' : 'block';
            });

            dropdownMenu.querySelectorAll('li').forEach(function (item) {
                item.addEventListener('click', function () {
                    dropdownToggle.textContent = this.textContent;
                    hiddenInput.value = this.getAttribute('data-value');
                    dropdownMenu.style.display = 'none';

                    dropdownToggle.className = 'dropdown-toggle ' + this.className;

                    saveTaskUpdate(dropdown.closest('tr'));
                });
            });
        });
    }

    function debounce(func, wait) {
        let timeout;
        return function (...args) {
            clearTimeout(timeout);
            timeout = setTimeout(() => func.apply(this, args), wait);
        };
    }

    function initializeTextareas(textareas) {
        textareas.forEach(function (textarea) {
            autoResize(textarea);
            updateCharCount(textarea);

            const debouncedSaveTaskUpdate = debounce(function () {
                saveTaskUpdate(textarea.closest('tr'));
            }, 2000); 

            textarea.addEventListener('input', function () {
                autoResize(textarea);
                updateCharCount(textarea);
                debouncedSaveTaskUpdate();
            });
        });
    }

    function autoResize(textarea) {
        textarea.style.height = '10px';
        textarea.style.height = textarea.scrollHeight + 'px';
    }

    function updateCharCount(textarea) {
        const maxLength = textarea.getAttribute('maxlength');
        const currentLength = textarea.value.length;
        const charCountDisplay = textarea.nextElementSibling;

        if (currentLength >= maxLength) {
            textarea.style.backgroundColor = 'rgba(255, 0, 0, 0.3)';
            charCountDisplay.textContent = `0 characters remaining`;
        } else {
            textarea.style.backgroundColor = '';
            charCountDisplay.textContent = '';
        }

        if (currentLength > maxLength) {
            textarea.value = textarea.value.substring(0, maxLength);
        }
    }

    function saveTaskUpdate(rowElement) {
        const taskChecklistId = rowElement.getAttribute('data-taskcheklist-id');
        const updatedStatus = rowElement.querySelector('input.status-dropdown').value;
        const updatedComment = rowElement.querySelector('textarea').value;

        const statusMap = {
            "TODO": 0,
            "SKIPPED": 1,
            "DONE": 2
        };

        console.log('Sending status:', statusMap[updatedStatus]);
        console.log(`Saving Task Update for Taskchecklist ID: ${taskChecklistId}`);
        console.log(`Updated Comment: ${updatedComment}`);

        fetch(`/api/taskchecklist/${taskChecklistId}`, {
            method: 'PUT',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({
                status: statusMap[updatedStatus],
                comment: updatedComment
            })
        })
            .then(response => {
                if (!response.ok) {
                    console.error('Failed to update the task checklist, server responded with:', response.status);
                    return response.text().then(text => Promise.reject(text));
                }
                return response.text();
            })
            .then(data => {
                console.log('Update successful');
            })
            .catch(error => {
                console.error('Error updating task checklist:', error);
            });
    }

});
