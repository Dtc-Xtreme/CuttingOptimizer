window.Alert = function(message) {
    alert(message);
}

window.Print = function(title) {
    changeTitle(title);
    print();
}

function changeTitle(title) {
    document.title = title;
}