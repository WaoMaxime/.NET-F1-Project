document.addEventListener("DOMContentLoaded", function () {
    document.querySelectorAll(".hp-button").forEach(button => {
        button.addEventListener("click", async function () {
            const carId = this.getAttribute("data-car-id");
            console.log("Button clicked for car ID:", carId);

            const newHp = document.getElementById("horsepower").value; 

            if (!newHp || isNaN(newHp) || parseInt(newHp) < 0) {
                alert("Please enter a valid HP value.");
                return;
            }

            try {
                const response = await fetch(`/api/F1Cars/${carId}`, {
                    method: "PUT",
                    headers: {
                        "Content-Type": "application/json",
                        "Authorization": `Bearer ${localStorage.getItem("token")}`
                    },
                    body: JSON.stringify({ F1CarHp: parseInt(newHp) })
                });
                
                if (response.status === 401) {
                    alert("You must be logged in to update this car.");
                } else if (response.status === 403) {
                    alert("You do not have permission to update this car.");
                } else if (response.ok) {
                    const data = await response.json();
                    alert(`HP updated successfully! New value: ${data.updatedHp}`);
                    document.getElementById("horsepower").value = data.updatedHp; 
                } else {
                    alert("Something went wrong.");
                }
            } catch (error) {
                console.error("Error updating HP:", error);
                alert("An error occurred while updating the HP.");
            }
        });
    });
});
