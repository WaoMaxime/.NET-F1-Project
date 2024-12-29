document.addEventListener("DOMContentLoaded", () => {
    const carId = document.getElementById("carId").value;

    async function loadTyres() {
        try {
            const response = await fetch(`/api/TyreApi/GetTyresForCar/${carId}`);
            if (!response.ok) {
                throw new Error(`Failed to fetch tyres: ${response.status}`);
            }

            const rawResponse = await response.json();
            console.log("Raw Tyres Response:", rawResponse);
            
            const tyres = rawResponse.$values ?? [];
            if (!Array.isArray(tyres)) {
                console.error("Tyres response is not an array:", tyres);
                return;
            }

            const tableBody = document.querySelector("#tyreTableBody tbody");
            if (!tableBody) {
                console.error("Element with ID 'tyreTableBody' not found in the DOM");
                return;
            }

            tableBody.innerHTML = "";
            tyres.forEach((tyre, index) => {
                const row = `
                <tr data-tyre-index="${index}">
                    <td>${tyre.tyre ?? "Unknown"}</td>
                    <td>${tyre.tyrePressure} PSI</td>
                    <td>${tyre.operationalTemperature} °C</td>
                </tr>`;
                tableBody.innerHTML += row;
            });
            
            const lastRow = tableBody.querySelector("tr:last-child");
            if (lastRow) {
                lastRow.scrollIntoView({ behavior: "smooth" });
            }
        } catch (error) {
            console.error("Error loading tyres:", error);
            alert("Failed to load tyres. Please try again later.");
        }
    }



    async function loadTyreTypes() {
        try {
            const response = await fetch("/api/TyreApi/GetTyreTypes");
            if (!response.ok) {
                throw new Error(`Failed to fetch tyre types: ${response.status}`);
            }

            const tyreTypes = await response.json();
            console.log("Tyre Types Loaded:", tyreTypes);

            const selectBox = document.getElementById("tyreType");
            if (!selectBox) {
                console.error("Element with ID 'tyreType' not found in the DOM");
                return;
            }

            selectBox.innerHTML = "";
            tyreTypes.forEach(type => {
                const option = document.createElement("option");
                option.value = type;
                option.textContent = type;
                selectBox.appendChild(option);
            });
        } catch (error) {
            console.error("Error loading tyre types:", error);
            alert("Failed to load tyre types. Please try again later.");
        }
    }

    async function addTyre() {
        const tyreType = document.getElementById("tyreType").value;
        const tyrePressure = document.getElementById("tyrePressure").value;
        const operationalTemperature = document.getElementById("operationalTemperature").value;

        const newTyre = {
            carId: parseInt(carId),
            tyre: tyreType,
            tyrePressure: parseInt(tyrePressure),
            operationalTemperature: parseInt(operationalTemperature),
        };

        try {
            console.log("Adding Tyre:", newTyre);
            const response = await fetch("/api/TyreApi/AddTyreToCar", {
                method: "POST",
                headers: { "Content-Type": "application/json" },
                body: JSON.stringify(newTyre),
            });

            if (!response.ok) {
                if (response.status === 409) {
                    const errorResponse = await response.json();
                    alert(errorResponse.message || "Duplicate tyre setup detected.");
                } else {
                    const errorResponse = await response.json();
                    console.error("Error response:", errorResponse);
                    alert(errorResponse.error || "Failed to add tyre.");
                }
                return;
            }

            alert("Tyre successfully added!");
            await loadTyres(); 
            loadTyreTypes();   
        } catch (error) {
            console.error("Error adding tyre:", error);
            alert("An unexpected error occurred while adding the tyre.");
        }
    }


    document.getElementById("addTyreButton").addEventListener("click", addTyre);
    
    loadTyres();
    loadTyreTypes();
});
