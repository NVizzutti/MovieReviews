$(document).ready(() => {
    $('th').click(function() {
        let table = $(this).parents('table').eq(0);
        this.direction = !this.direction;
        let rows = table.find('tr:gt(0)').toArray().sort(comparator($(this).index()));
        if (!this.direction) {
            rows = rows.reverse();
        }
        for (let i = 0; i < rows.length; i++) {
            table.append(rows[i]);
        }
    })
    function comparator(idx) {
        return function (a, b) {
            let valA = getCellValue(a, idx), valB = getCellValue(b, idx);
            return $.isNumeric(valA) && $.isNumeric(valB) ? valA - valB : valA.localeCompare(valB)
        }
    }
    function getCellValue(row, index) { return $(row).children('td').eq(index).html() }
})