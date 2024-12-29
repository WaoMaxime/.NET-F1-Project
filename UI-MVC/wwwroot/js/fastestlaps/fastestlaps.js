async function loadFastestLaps() {
    try {
        const response = await fetch('/api/FastestLapApi');
        if (!response.ok) {
            throw new Error(`Failed to fetch fastest laps: ${response.status}`);
        }

        const data = await response.json();
        const tableBody = document.getElementById('fastestLapTableBody');
        tableBody.innerHTML = '';

        data.forEach(lap => {
            const row = `
                <tr>
                    <td>${lap.car?.chasis ?? "Unknown"}</td>
                    <td>${lap.race?.name ?? "Unknown"}</td>
                    <td>${lap.lapTime}</td>
                    <td>${new Date(lap.dateOfRecord).toLocaleDateString()}</td>
                </tr>`;
            tableBody.innerHTML += row;
        });
    } catch (error) {
        console.error("Error loading fastest laps:", error);
    }
}

document.getElementById('reloadButton').addEventListener('click', loadFastestLaps);

loadFastestLaps();
