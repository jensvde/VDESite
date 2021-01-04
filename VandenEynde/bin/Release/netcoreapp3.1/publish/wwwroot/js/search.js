console.log("loaded");
document.addEventListener('load', () => {
    var table = document.getElementById('table');
    console.log('hi!yjl' + table);
    for (row in table.rows) {
        if (row.cells.length !== 0) {
            console.log(row.cells[0].innerText)
        }
    }
});

var table = document.getElementById('table');
console.log('hi!yjl' + table);