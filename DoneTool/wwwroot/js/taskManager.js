document.addEventListener('DOMContentLoaded', function () {
    const dropdowns = document.querySelectorAll('.custom-dropdown');
    const modal = document.getElementById("commentModal");
    const closeBtn = document.querySelector(".close");
    const modalTextarea = document.getElementById('skipComment');
    const reasonsDropdown = document.getElementById('commonReasonsDropdown');
    let activeRow;

    function closeModalAndDropdown() {
        modal.style.display = "none";
        if (activeRow) {
            const dropdownMenu = activeRow.querySelector('.dropdown-menu');
            dropdownMenu.style.display = 'none';
        }
    }

    closeBtn.onclick = function () {
        closeModalAndDropdown();
    }

    window.onclick = function (event) {
        if (event.target == modal) {
            closeModalAndDropdown();
        }
    }

    dropdowns.forEach(function (dropdown) {
        const dropdownMenu = dropdown.querySelector('.dropdown-menu');
        const statusButton = dropdown.querySelector('.status-button');
        const arrowButton = dropdown.querySelector('.arrow-button');
        const hiddenInput = dropdown.querySelector('input[type="hidden"]');

        dropdownMenu.querySelectorAll('li').forEach(function (item) {
            item.addEventListener('click', function () {
                const statusValue = this.getAttribute('data-value');

                if (statusValue === 'SKIPPED') {
                    activeRow = dropdown.closest('tr');
                    modalTextarea.value = ''; 
                    populateReasonsDropdown(activeRow);
                    modal.style.display = "block";
                } else {
                    hiddenInput.value = statusValue;
                    statusButton.textContent = statusValue;
                    statusButton.className = 'status-button status-' + statusValue.toLowerCase();
                    arrowButton.className = 'arrow-button status-' + statusValue.toLowerCase();

                    dropdownMenu.style.display = 'none';
                    saveTaskUpdate(dropdown.closest('tr'));
                }
            });
        });
    });

    function populateReasonsDropdown(row) {
        const reasonsAttribute = row.getAttribute('data-skip-reasons');

        if (!reasonsAttribute) {
            console.log('No reasons found for this task.');
            return;
        }

        const reasons = reasonsAttribute.split(',');

        if (!reasons || reasons.length === 0) {
            console.log('No reasons found for this task.');
            return;
        }

        reasonsDropdown.innerHTML = '<option value="">Select a reason...</option>';

        reasons.forEach(function (reason) {
            const option = document.createElement('option');
            option.value = reason.trim();
            option.textContent = reason.trim();
            reasonsDropdown.appendChild(option);
        });

        reasonsDropdown.onchange = function () {
            const selectedReason = this.value;
            modalTextarea.value = selectedReason; 
        };
    }

    modalTextarea.addEventListener('input', function () {
        reasonsDropdown.value = ''; 
    });

    document.getElementById('submitSkip').onclick = function () {
        const comment = modalTextarea.value;

        if (comment.length < 10) {
            alert('Please enter at least 10 characters for the comment.');
            return;
        }

        const dropdown = activeRow.querySelector('.custom-dropdown');
        const hiddenInput = dropdown.querySelector('input[type="hidden"]');
        const statusButton = dropdown.querySelector('.status-button');
        const arrowButton = dropdown.querySelector('.arrow-button');

        hiddenInput.value = "SKIPPED";
        statusButton.textContent = "SKIPPED";
        statusButton.className = 'status-button status-skipped';

        arrowButton.className = 'arrow-button status-skipped';

        const commentField = activeRow.querySelector('textarea');
        commentField.value = comment;

        closeModalAndDropdown();

        modalTextarea.value = '';

        saveTaskUpdate(activeRow);
    };

    const textareas = document.querySelectorAll('tbody textarea');
    textareas.forEach(function (textarea) {
        const rowElement = textarea.closest('tr');
        const debouncedSaveTaskUpdate = debounce(function () {
            if (rowElement) {
                saveTaskUpdate(rowElement);
            } else {
                console.error("No row element found for textarea update. Textarea content:", textarea.value);
            }
        }, 2000);

        textarea.addEventListener('input', function () {
            debouncedSaveTaskUpdate();
        });
    });

    function debounce(func, delay) {
        let timeout;
        return function () {
            const context = this;
            const args = arguments;
            clearTimeout(timeout);
            timeout = setTimeout(() => func.apply(context, args), delay);
        };
    }

    const customReloadModal = document.getElementById('customReloadModal');
    if (customReloadModal) {
        customReloadModal.style.display = 'none';
    }

    const guardInputs = document.querySelectorAll('.guard-search');
    guardInputs.forEach(function (guardInput) {
        const dropdownMenu = guardInput.nextElementSibling;
        dropdownMenu.querySelectorAll('li').forEach(function (item) {
            item.addEventListener('click', function () {
                guardInput.value = this.getAttribute('data-value');
                const rowElement = guardInput.closest('tr');
                saveTaskUpdate(rowElement);
            });
        });
    });
});

function toggleStatus(button) {
    const dropdown = button.closest('.custom-dropdown');
    const hiddenInput = dropdown.querySelector('input[type="hidden"]');
    const statusButton = dropdown.querySelector('.status-button');
    const arrowButton = dropdown.querySelector('.arrow-button');
    const currentStatus = hiddenInput.value;

    if (currentStatus !== "DONE") {
        hiddenInput.value = "DONE";
        statusButton.textContent = "DONE";
        statusButton.classList.remove('status-todo', 'status-skipped');
        statusButton.classList.add('status-done');

        arrowButton.classList.remove('status-todo', 'status-skipped');
        arrowButton.classList.add('status-done');

        saveTaskUpdate(dropdown.closest('tr'));
    }
}

function toggleDropdownMenu(button) {
    const dropdownMenu = button.closest('.custom-dropdown').querySelector('.dropdown-menu');
    const dropdown = button.closest('.custom-dropdown');
    const statusButton = dropdown.querySelector('.status-button');
    const arrowButton = button;

    arrowButton.className = statusButton.className.replace('status-button', 'arrow-button');

    dropdownMenu.style.display = dropdownMenu.style.display === 'block' ? 'none' : 'block';
}

function saveTaskUpdate(rowElement) {
    if (!rowElement) {
        console.error('saveTaskUpdate called with null rowElement');
        return;
    }

    const taskChecklistId = rowElement.getAttribute('data-taskchecklist-id');
    const updatedStatus = rowElement.querySelector('input.status-dropdown').value;
    const updatedComment = rowElement.querySelector('textarea').value;
    const updatedGuard = rowElement.querySelector('input.guard-search').value;
    let lastUpdated = rowElement.getAttribute('data-last-updated');

    if (!lastUpdated) {
        console.error('LastUpdated is null or undefined');
        return;
    }

    console.log(`Sending LastUpdated: ${lastUpdated}`);

    const statusMap = {
        "TODO": 0,
        "SKIPPED": 1,
        "DONE": 2
    };

    fetch(`/api/taskchecklist/${taskChecklistId}`, {
        method: 'PUT',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify({
            status: statusMap[updatedStatus],
            comment: updatedComment,
            guard: updatedGuard,
            lastUpdated: lastUpdated
        })
    })
        .then(response => {
            if (response.status === 409) {
                console.log(response.text().then(text => Promise.reject(text)));
                showCustomReloadModal();
                return;
            }

            if (!response.ok) {
                console.error('Failed to update the task checklist, server responded with:', response.status);
                return response.text().then(text => Promise.reject(text));
            }

            return response.json();
        })
        .then(data => {
            if (!data) {
                throw new Error("Received undefined or null data from the server");
            }
            console.log('Update successful');
            console.log('Received Data:', data);
            console.log(`Received LastUpdated: ${data.lastUpdated}`);
            rowElement.setAttribute('data-last-updated', data.lastUpdated);
        })
        .catch(error => {
            console.error('Error updating task checklist:', error);
        });
}

function showCustomReloadModal() {
    const modal = document.getElementById('customReloadModal');
    const refreshButton = document.getElementById('customReloadButton');

    modal.style.display = 'block';

    refreshButton.onclick = function () {
        window.location.reload();
    };
}
