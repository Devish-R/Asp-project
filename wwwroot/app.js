const employeeForm = document.getElementById("employeeForm");

employeeForm.addEventListener("submit", async function (event) {

    event.preventDefault();

    const employee = {
        name: document.getElementById("name").value,
        department: document.getElementById("department").value,
        salary: Number(document.getElementById("salary").value)
    };

    const message = document.getElementById("message");

    try {

        const response = await fetch("/api/employees", {
            method: "POST",

            headers: {
                "Content-Type": "application/json"
            },

            body: JSON.stringify(employee)
        });

        if (response.ok) {

            message.textContent =
                "Employee added successfully.";

            employeeForm.reset();

            loadEmployees();

        } else {

            const result = await response.json().catch(() => null);

            message.textContent =
                result?.message || "Failed to add employee.";

        }

    } catch (error) {

        console.error(error);

        message.textContent =
            "Error connecting to server.";

    }

});


async function loadEmployees() {

    try {

        const response = await fetch("/api/employees");

        if (!response.ok) {
            throw new Error("Failed to load employees");
        }

        const employees = await response.json();

        const tableBody =
            document.getElementById("employeeTableBody");

        tableBody.innerHTML = "";

        employees.forEach(employee => {

            const row = document.createElement("tr");

            row.innerHTML = `
                <td>${employee.id}</td>
                <td>${employee.name}</td>
                <td>${employee.department}</td>
                <td>${employee.salary}</td>
            `;

            tableBody.appendChild(row);

        });

    } catch (error) {

        console.error(error);

    }

}


const uploadForm = document.getElementById("uploadForm");

uploadForm.addEventListener("submit", async function (event) {

    event.preventDefault();

    const fileInput =
        document.getElementById("fileInput");

    const message =
        document.getElementById("uploadMessage");

    if (fileInput.files.length === 0) {

        message.textContent =
            "Please select a file.";

        return;
    }

    const formData = new FormData();

    formData.append(
        "file",
        fileInput.files[0]
    );

    try {

        const response = await fetch(
            "/api/files/upload",
            {
                method: "POST",
                body: formData
            }
        );

        const result =
            await response.json().catch(() => null);

        if (response.ok) {

            message.textContent =
                "File uploaded successfully to S3.";

            fileInput.value = "";

        } else {

            message.textContent =
                result?.message || "File upload failed.";

        }

    } catch (error) {

        console.error(error);

        message.textContent =
            "Error connecting to server.";

    }

});