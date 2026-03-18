// ========================================
// API BASE
// ========================================
const apiUrl = "http://localhost:5054/api"; // Base URL for backend API


// ========================================
// HELPER FUNCTION
// ========================================
async function fetchData(url) {
    try {
        const res = await fetch(url); // Send GET request

        if (!res.ok) { // Check if response failed
            console.error("API error:", res.status);
            return null;
        }

        return await res.json(); // Convert response to JSON
    } catch (err) {
        console.error("Fetch error:", err); // Catch network error
        return null;
    }
}


// ========================================
// DASHBOARD
// ========================================
async function loadDashboard() {

    // Fetch all data
    const vehicles = await fetchData(`${apiUrl}/Vehicles`);
    const drivers = await fetchData(`${apiUrl}/Drivers`);
    const trips = await fetchData(`${apiUrl}/Trips`);
    const fuel = await fetchData(`${apiUrl}/FuelLogs`);
    const accidents = await fetchData(`${apiUrl}/Accidents`);

    // Show vehicle count
    if (document.getElementById("vehicleCount"))
        document.getElementById("vehicleCount").innerText = vehicles?.length ?? 0;

    // Show driver count
    if (document.getElementById("driverCount"))
        document.getElementById("driverCount").innerText = drivers?.length ?? 0;

    // Show trip count
    if (document.getElementById("tripCount"))
        document.getElementById("tripCount").innerText = trips?.length ?? 0;

    // Show total fuel
    if (document.getElementById("fuelCount")) {
        const totalFuel = fuel?.reduce((s, f) => s + f.liters, 0) ?? 0; // Sum liters
        document.getElementById("fuelCount").innerText = totalFuel + " L";
    }

    // Show accident count
    if (document.getElementById("accidentCount"))
        document.getElementById("accidentCount").innerText = accidents?.length ?? 0;
}


// ========================================
// DRIVERS
// ========================================
let editingId = null; // Store current editing driver ID

async function loadDrivers() {

    const table = document.getElementById("driverTable");
    if (!table) return; // Stop if not on driver page

    const data = await fetchData(`${apiUrl}/Drivers`);
    console.log("Drivers:", data);

    table.innerHTML = ""; // Clear old data

    // If no data
    if (!data || data.length === 0) {
        table.innerHTML = "<tr><td colspan='4'>No data</td></tr>";
        return;
    }

    // Render each driver
    data.forEach(d => {
        table.innerHTML += `
        <tr>
            <td>${d.id}</td>
            <td>${d.fullName}</td>
            <td>${d.licenseNumber}</td>
            <td>
                <button onclick="editDriver(${d.id}, '${d.fullName}', '${d.licenseNumber}')">Edit</button>
                <button onclick="deleteDriver(${d.id})">Delete</button>
            </td>
        </tr>`;
    });
}

// ADD DRIVER
async function addDriver() {

    const name = document.getElementById("name").value;
    const license = document.getElementById("license").value;

    // Validate input
    if (!name || !license) {
        alert("Please fill all fields!");
        return;
    }

    // Send POST request
    await fetch(`${apiUrl}/Drivers`, {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({
            fullName: name,
            licenseNumber: license,
            licenseExpiry: new Date() // Default expiry date
        })
    });

    resetForm(); // Reset form
    loadDrivers(); // Reload table
    loadDashboard(); // Update dashboard
}

// SET EDIT MODE
function editDriver(id, name, license) {

    editingId = id; // Save ID

    // Fill form
    document.getElementById("name").value = name;
    document.getElementById("license").value = license;

    // Change button to update
    const btn = document.querySelector(".btn-primary");
    btn.innerText = "Update Driver";
    btn.onclick = updateDriver;
}

// UPDATE DRIVER
async function updateDriver() {

    const name = document.getElementById("name").value;
    const license = document.getElementById("license").value;

    await fetch(`${apiUrl}/Drivers/${editingId}`, {
        method: "PUT",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({
            id: editingId,
            fullName: name,
            licenseNumber: license,
            licenseExpiry: new Date()
        })
    });

    resetForm();
    loadDrivers();
    loadDashboard();
}

// DELETE DRIVER
async function deleteDriver(id) {

    if (!confirm("Delete driver?")) return; // Confirm before delete

    await fetch(`${apiUrl}/Drivers/${id}`, {
        method: "DELETE"
    });

    loadDrivers();
    loadDashboard();
}

// RESET FORM
function resetForm() {

    editingId = null; // Clear editing state

    document.getElementById("name").value = "";
    document.getElementById("license").value = "";

    const btn = document.querySelector(".btn-primary");
    btn.innerText = "Add Driver";
    btn.onclick = addDriver;
}


// ========================================
// FUEL LOGS
// ========================================
async function loadFuelLogs() {

    const table = document.getElementById("fuelTable");
    if (!table) return;

    const data = await fetchData(`${apiUrl}/FuelLogs`);
    table.innerHTML = "";

    // Render fuel logs
    data?.forEach(f => {
        table.innerHTML += `
        <tr>
            <td>${f.id}</td>
            <td>${f.vehicleId}</td>
            <td>${new Date(f.fuelDate).toLocaleDateString()}</td> <!-- Format date -->
            <td>${f.liters}</td>
            <td>${f.totalCost}</td>
            <td>
                <button onclick="editFuel(${f.id})">Edit</button>
                <button onclick="deleteFuelLog(${f.id})">Delete</button>
            </td>
        </tr>`;
    });
}

// SAVE FUEL (ADD / UPDATE)
async function saveFuel() {

    const id = document.getElementById("fuelId")?.value;

    // Create object
    const data = {
        vehicleId: parseInt(document.getElementById("fuel_vehicleId").value),
        liters: parseFloat(document.getElementById("liters").value),
        totalCost: parseFloat(document.getElementById("fuel_cost").value),
        fuelDate: new Date() // Current time
    };

    if (id) {
        // UPDATE
        await fetch(`${apiUrl}/FuelLogs/${id}`, {
            method: "PUT",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify({ id: parseInt(id), ...data })
        });
    } else {
        // CREATE
        await fetch(`${apiUrl}/FuelLogs`, {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify(data)
        });
    }

    clearFuelForm();
    loadFuelLogs();
    loadDashboard();
}

// DELETE FUEL
async function deleteFuelLog(id) {

    if (!confirm("Delete fuel log?")) return;

    await fetch(`${apiUrl}/FuelLogs/${id}`, { method: "DELETE" });

    loadFuelLogs();
    loadDashboard();
}

// EDIT FUEL
async function editFuel(id) {

    const f = await fetchData(`${apiUrl}/FuelLogs/${id}`);

    // Fill form
    document.getElementById("fuelId").value = f.id;
    document.getElementById("fuel_vehicleId").value = f.vehicleId;
    document.getElementById("liters").value = f.liters;
    document.getElementById("fuel_cost").value = f.totalCost;
}

// CLEAR FORM
function clearFuelForm() {

    document.getElementById("fuelId").value = "";
    document.getElementById("fuel_vehicleId").value = "";
    document.getElementById("liters").value = "";
    document.getElementById("fuel_cost").value = "";
}


// ========================================
// ACCIDENTS
// ========================================
async function loadAccidents() {

    const table = document.getElementById("accidentTable");
    if (!table) return;

    const data = await fetchData(`${apiUrl}/Accidents`);
    table.innerHTML = "";

    // Render accident list
    data?.forEach(a => {
        table.innerHTML += `
        <tr>
            <td>${a.id}</td>
            <td>${a.vehicleId}</td>
            <td>${a.driverId}</td>
            <td>${a.location}</td>
            <td>${a.damageCost}</td>
            <td>
                <button onclick="editAccident(${a.id})">Edit</button>
                <button onclick="deleteAccident(${a.id})">Delete</button>
            </td>
        </tr>`;
    });
}


// ========================================
// CHART (ACCIDENTS)
// ========================================
async function loadAccidentChart() {

    const canvas = document.getElementById("accidentChart");
    if (!canvas) return;

    const data = await fetchData(`${apiUrl}/Accidents`);
    if (!data || data.length === 0) return;

    const stats = {};

    // Count accidents by date
    data.forEach(a => {
        const date = new Date(a.accidentDate).toLocaleDateString();
        stats[date] = (stats[date] || 0) + 1;
    });

    // Draw chart
    new Chart(canvas, {
        type: "line",
        data: {
            labels: Object.keys(stats),
            datasets: [{
                label: "Accidents",
                data: Object.values(stats),
                borderWidth: 2
            }]
        }
    });
}


// ========================================
// AUTO LOAD
// ========================================
document.addEventListener("DOMContentLoaded", () => {

    loadDashboard(); // Load dashboard

    // Load specific pages only if element exists
    if (document.getElementById("driverTable")) loadDrivers();
    if (document.getElementById("fuelTable")) loadFuelLogs();
    if (document.getElementById("accidentTable")) loadAccidents();
    if (document.getElementById("accidentChart")) loadAccidentChart();
});