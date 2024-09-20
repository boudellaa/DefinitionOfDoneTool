function updateActionButtons() {
    const rows = document.querySelectorAll('tr[data-taskchecklist-id]');
    let shouldDisableFollowing = false;

    rows.forEach((row, index) => {
        const actionButton = row.querySelector('.action-button-wrapper .action-button');
        actionButton.disabled = true; 
        actionButton.textContent = "No Action";

        if (index === 0) {
            actionButton.textContent = "Schedule Meeting";
            actionButton.disabled = false;
        } else {
            const prevRowStatus = rows[index - 1].querySelector('input.status-dropdown').value;
            const stepName = row.querySelector('.step-title').textContent.trim();

            if (stepName.includes("Code review")) {
                actionButton.textContent = "Send to CR";
            } else if (stepName.includes("Quality assurance")) {
                actionButton.textContent = "Send to QA";
            }
            if (shouldDisableFollowing || prevRowStatus !== "DONE") {
                shouldDisableFollowing = true;
            }

            if (!shouldDisableFollowing) {
                if (stepName.includes("Code review")) {
                    actionButton.disabled = false;
                } else if (stepName.includes("Quality assurance")) {
                    actionButton.disabled = false;
                }
            }
        }
    });
}

document.addEventListener('DOMContentLoaded', function () {
    updateActionButtons();

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
                    saveTaskUpdate(dropdown.closest('tr'), updateActionButtons());
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
        autoResize(textarea);
        const debouncedSaveTaskUpdate = debounce(function () {
            if (rowElement) {
                saveTaskUpdate(rowElement);
            } else {
                console.error("No row element found for textarea update. Textarea content:", textarea.value);
            }
        }, 2000);

        textarea.addEventListener('input', function () {
            autoResize(textarea);
            debouncedSaveTaskUpdate();
        });
    });

    function autoResize(textarea) {
        textarea.style.height = '41.6';

        let newHeight = textarea.scrollHeight;

        if (newHeight > 125) {
            textarea.style.height = '125px';
            textarea.style.overflowY = 'auto';
        } else {
            textarea.style.height = newHeight + 'px';
            textarea.style.overflowY = 'hidden';
        }
    }

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

        saveTaskUpdate(dropdown.closest('tr'), updateActionButtons());
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

function saveTaskUpdate(rowElement, callback) {
    const taskChecklistId = rowElement.getAttribute('data-taskchecklist-id');
    const updatedStatus = rowElement.querySelector('input.status-dropdown').value;
    const updatedComment = rowElement.querySelector('textarea').value;
    const updatedGuard = rowElement.querySelector('input.guard-search').value;
    let lastUpdated = rowElement.getAttribute('data-last-updated');

    // Handle duplicate fields (if they exist)
    const duplicateStatusElement = rowElement.querySelector('.status-button.duplicate');
    const duplicateCommentElement = rowElement.querySelector('textarea.duplicate');
    const duplicateGuardElement = rowElement.querySelector('input.guard-search.duplicate');

    const duplicateExists = duplicateStatusElement && duplicateCommentElement && duplicateGuardElement;

    const statusMap = {
        "TODO": 0,
        "SKIPPED": 1,
        "DONE": 2
    };

    const originalTaskUpdateData = {
        status: statusMap[updatedStatus],
        comment: updatedComment,
        guard: updatedGuard,
        lastUpdated: lastUpdated
    };

    // First, update the original task checklist
    fetch(`/api/taskchecklist/${taskChecklistId}`, {
        method: 'PUT',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(originalTaskUpdateData)
    })
        .then(response => {
            if (response.status === 409) {
                showCustomReloadModal();
                return;
            }
            if (!response.ok) {
                return response.text().then(text => Promise.reject(text));
            }
            return response.json();
        })
        .then(data => {
            if (!data) {
                throw new Error("Failed to update the original task checklist.");
            }

            rowElement.setAttribute('data-last-updated', data.lastUpdated);

            // Now handle updating the duplicate if it exists
            if (duplicateExists) {
                const duplicateStatus = duplicateStatusElement.value;
                const duplicateComment = duplicateCommentElement.value;
                const duplicateGuard = duplicateGuardElement.value;

                const duplicateTaskUpdateData = {
                    status: statusMap[duplicateStatus],
                    comment: duplicateComment,
                    guard: duplicateGuard,
                    lastUpdated: lastUpdated
                };

                // Fetch duplicate task ID via the API
                fetch(`/api/taskchecklist/${taskChecklistId}/duplicate`, {
                    method: 'GET'
                })
                    .then(response => response.json())
                    .then(duplicateData => {
                        if (duplicateData && duplicateData.id) {
                            console.log("Duplicate ID:", duplicateData.id);
                            // Update the duplicate task checklist
                            fetch(`/api/taskchecklist/${duplicateData.ID}`, {
                                method: 'PUT',
                                headers: {
                                    'Content-Type': 'application/json'
                                },
                                body: JSON.stringify(duplicateTaskUpdateData)
                            })
                                .then(duplicateResponse => {
                                    if (!duplicateResponse.ok) {
                                        return duplicateResponse.text().then(text => Promise.reject(text));
                                    }
                                    return duplicateResponse.json();
                                })
                                .then(duplicateUpdatedData => {
                                    if (typeof callback === "function") {
                                        callback();
                                    }
                                })
                                .catch(error => {
                                    console.error('Error updating duplicate task checklist:', error);
                                });
                        }
                    })
                    .catch(error => {
                        console.error('Error fetching duplicate task checklist:', error);
                    });
            }
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

function duplicateTask(id, button) {
    $.post(`/api/taskchecklist/${id}/duplicate`, function (response) {
        if (!response || !response.id) {
            console.error("Failed to duplicate the task.");
            return;
        }

        const taskData = tasks.find(t => t.TaskId === id);

        if (!taskData) {
            console.error("Task data not found for ID:", id);
            return;
        }

        var row = $(button).closest('tr');

        var deleteButton = `<button onclick="deleteTask('${response.id}', this)" class="btn btn-danger btn-right delete-btn">-</button>`;
        $(button).after(deleteButton);

        var statusFieldHtml = `
            <div class="custom-dropdown duplicated-field">
                <div class="status-button-wrapper">
                    <button class="status-button status-todo" onclick="toggleStatus(this)">TODO</button>
                    <button class="arrow-button status-todo" onclick="toggleDropdownMenu(this)">
                        <span class="dropdown-arrow">▼</span>
                    </button>
                </div>
                <ul class="dropdown-menu">
                    <li class="status-todo" data-value="TODO">TODO</li>
                    <li class="status-skipped" data-value="SKIPPED">SKIPPED</li>
                </ul>
                <input type="hidden" name="status" class="form-control status-dropdown" value="TODO">
            </div>
        `;

        row.find('td.status-cell').append(statusFieldHtml);

        var suggestedGuardsHtml = '<li class="dropdown-section">Suggested</li>';
        taskData.SuggestedGuards.forEach(function (guard) {
            suggestedGuardsHtml += `<li class="guard-option" data-value="${guard}" onclick="selectGuard(this, '${response.id}')">${guard}</li>`;
        });

        var otherGuardsHtml = '<li class="dropdown-section">Other</li>';
        taskData.OtherGuards.forEach(function (guard) {
            otherGuardsHtml += `<li class="guard-option" data-value="${guard}" onclick="selectGuard(this, '${response.id}')">${guard}</li>`;
        });

        row.find('td.guard-cell').append(`
            <div class="custom-guard-dropdown duplicated-field">
                <input type="text" class="form-control guard-search" placeholder="Search Guards" onclick="toggleGuardDropdown(this)" oninput="filterGuardOptions(this)">
                <ul class="guard-dropdown-menu">
                    ${suggestedGuardsHtml}
                    ${otherGuardsHtml}
                </ul>
            </div>
        `);

        row.find('td.comment-cell').append(`
            <textarea class="form-control duplicate duplicated-field" placeholder="Enter your comment"></textarea>
        `);
    }).fail(function () {
        console.error("Failed to duplicate task via API.");
    });
}

function deleteTask(id, button) {
    $.ajax({
        url: `/api/taskchecklist/${id}/duplicate`,
        type: 'DELETE',
        success: function (result) {
            var row = $(button).closest('tr');
            row.find('.duplicated-field').remove();
            $(button).remove();
        }
    });
}

function scheduleMeeting(taskTitle, taskCreator, dpo) {
    var subject = `Kickoff for ${taskTitle}`;
    var body = "This is a kickoff meeting for the task.";
    var to = taskCreator; 
    var cc = `${dpo}`;

    var mailto_link = `mailto:${to}?subject=${encodeURIComponent(subject)}&cc=${encodeURIComponent(cc)}&body=${encodeURIComponent(body)}`;
    window.location.href = mailto_link;
}

function openOneNote() {
    var oneNoteLink = "https://skylinebe.sharepoint.com/sites/DeployandAccelerate/EcsProductsNotes/Shared%20Documents/Forms/AllItems.aspx?viewpath=%2Fsites%2FDeployandAccelerate%2FEcsProductsNotes%2FShared%20Documents%2FForms%2FAllItems%2Easpx&id=%2Fsites%2FDeployandAccelerate%2FEcsProductsNotes%2FShared%20Documents%2FNotebooks&viewid=7ec3d9b3%2D4c67%2D4ac9%2Db72b%2D1a46fb69115c";
    window.open(oneNoteLink, "_blank");
}