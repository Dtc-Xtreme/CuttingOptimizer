//window.addEventListener('DOMContentLoaded', function () {
//    let dataUrl = 'https://localhost:44308/json/data.json';

//    let productData;

//    let canvas = d3.select("#canvas");

//    let drawTreeMap = () => {
//        let hierarchy = d3.hierarchy(productData, (node) => {
//            return node['children']
//        }).sum((node) => {
//            return node['value']
//        });

//        let createTreeMap = d3.treemap().size([1000, 600]);

//        createTreeMap(hierarchy);

//        let productTiles = hierarchy.leaves();
//        console.log(productTiles);

//        let block = canvas.selectAll('g')
//            .data(productTiles)
//            .enter()
//            .append('g')
//            .attr('transform', (movie) => {
//                return 'translate(' + movie['x0'] + ', ' + movie['y0'] + ')';
//            });

//        block.append('rect')
//            .attr('class', 'tile')
//            .attr('width', (movie) => {
//                return movie['x1'] - movie['x0'];
//            })
//            .attr('height', (movie) => {
//                return movie['y1'] - movie['y0'];
//            });

//    }

//    d3.json(dataUrl).then(
//        (data, error) => {
//            if (error) {
//                console.log(error);
//            } else {
//                productData = data;
//                console.log(data);
//                drawTreeMap();
//            }
//        }
//    );
//});

function createInputForPlates() {
    const form = document.getElementById("plateForm");
    let formElements = document.getElementById("plateFormBaseElements");
    let newForm = formElements.cloneNode(true);
    const id = "pl" + (form.childElementCount - 3);
    newForm.id = id;

    // Change IDs
    const elementNr = form.childElementCount - 2;
    const regex1 = /^Plates_[0-9]+/g;
    const regex2 = /^Plates\[[0-9]+\]/g;

    for (let i = 0; i < newForm.children.length; i++) {
        //alert(newForm.children[i].id);
        let newId = newForm.children[i].id;
        newId = newId.replaceAll(regex1, "Plates_" + elementNr);
        let newName = newForm.children[i].name;
        newName = newName.replaceAll(regex2, "Plates[" + elementNr + "]");
        newForm.children[i].id = newId;
        newForm.children[i].name = newName;
        newForm.children[i].value = "";
    }

    newForm.children[0].value = 1;
    newForm.children[5].value = false;

    // Add remove button
    const button = document.createElement("button");
    button.className = "btn btn-danger";
    button.setAttribute("onclick", "removeThisInputWithId(" + id + ")");
    button.innerHTML = "X";
    button.type = "button";

    newForm.appendChild(button);
    form.appendChild(newForm);
}
function createInputForProducts() {
    const form = document.getElementById("productForm");
    let formElements = document.getElementById("productFormBaseElements");
    let newForm = formElements.cloneNode(true);
    const id = "pd" + (form.childElementCount - 3);
    newForm.id = id;

    // Change IDs
    const elementNr = form.childElementCount - 2;
    const regex1 = /^Products_[0-9]+/g;
    const regex2 = /^Products\[[0-9]+\]/g;

    for (let i = 0; i < newForm.children.length; i++) {
        //alert(newForm.children[i].id);
        let newId = newForm.children[i].id;
        newId = newId.replaceAll(regex1, "Products_" + elementNr);
        let newName = newForm.children[i].name;
        newName = newName.replaceAll(regex2, "Products[" + elementNr + "]");
        newForm.children[i].id = newId;
        newForm.children[i].name = newName;
        newForm.children[i].value = "";
    }

    newForm.children[0].value = 1;

    // Add remove button
    const button = document.createElement("button");
    button.className = "btn btn-danger";
    button.setAttribute("onclick", "removeThisInputWithId(" + id + ")");
    button.innerHTML = "X";
    button.type = "button";

    newForm.appendChild(button);
    form.appendChild(newForm);
}
function removeThisInputWithId(id) {
    id.remove();
}

// Classes
class Saw {
    constructor(serial, thickness) {
        this.serial = serial;
        this.thickness = thickness;
    }
}
class Plate {
    totalArea = 0;
    unoccupiedArea = 0;
    products = [];

    constructor(quantity, serial, width, length, height, trim, vineer, totalArea) {
        this.quantity = quantity;
        this.serial = serial;
        this.width = width;
        this.length = length;
        this.height = height;
        this.trim = trim;
        this.vineer = vineer;
        this.totalArea = totalArea;
        this.unoccupiedArea = totalArea;
    }

    getWidthWithTrim() {
        return this.width - this.trim;
    }
    getLengthWithTrim() {
        return this.length - this.trim;
    }
    getAreaWithTrim() {
        return (this.getWidthWithTrim() * this.getLengthWithTrim());
    }

    setProduct(product) {
        this.products.push(product);
        this.unoccupiedArea -= product.totalArea;
    }
}
class Product {
    totalArea = 0;
    canvasX = 0;
    canvasY = 0;

    constructor(quantity, width, length, height, info) {
        this.quantity = quantity;
        this.width = width;
        this.length = length;
        this.height = height;
        this.info = info;
    }
}

let svgArea;
let canvasArea;
let response;

let saw;
let plates;
let products;

window.addEventListener('DOMContentLoaded', function () {
    this.svgArea = document.getElementById("svgArea");
    this.canvasArea = document.getElementById("canvasArea");
    this.response = document.getElementById("CalculatorResponse");
});

// Tools
function calculateArea(width, length) {
    return width * length;
}
function calculateIfFits(plate, product) {
    let notUsableArea;
    let horizontal = false;
    let vertical = false;

    if (plate.products.length == 0) {
        horizontal = plate.getWidthWithTrim() > product.width ? true : false;
        vertical = plate.getLengthWithTrim() > product.length ? true : false;
    } else {
        const maxWidth = plate.products.reduce((a, b) => {
            return Math.max(a.width, b.width);
        })

        var a = "";
    }

    return horizontal && vertical;
}

function createSaw(input) {
    let saw = new Saw(document.getElementById("Saw_ID").value, document.getElementById("Saw_Thickness").value);
    return saw;
}
function createPlates() {
    let plates = [];

    let platesInput = document.querySelectorAll('[id^="Plates_"');

    let counter = platesInput.length / 7;
    for (let x = 0; x < platesInput.length; x = x + 7) {
        //console.log(x);
        plates.push(new Plate(platesInput[x].value, platesInput[x + 1].value, platesInput[x + 2].value, platesInput[x + 3].value, platesInput[x + 4].value, platesInput[x + 5].value, platesInput[x + 6].checked, calculateArea(platesInput[x + 2].value, platesInput[x + 3].value)));
    }
    //console.log(plates);
    return plates;
}
function createProducts(input) {
    let products = [];

    let productsInput = document.querySelectorAll('[id^="Products_"');

    let counter = productsInput.length / 5;
    for (let x = 0; x < productsInput.length; x = x + 5) {
        //console.log(x);
        let prod = new Product(productsInput[x].value, productsInput[x + 1].value, productsInput[x + 2].value, productsInput[x + 3].value, productsInput[x + 4].value);
        prod.totalArea = calculateArea(prod.width, prod.length);
        products.push(prod);
    }
    //console.log(products);
    return products;
}

function createCanvas(plateArray) {
    const diffWidth = plateArray.reduce((a, b) => {
        return Math.max(a.width, b.width) / 100;
    })

    const maxWidth = plateArray.reduce((a, b) => {
        return Math.max(a.width, b.width);
    })

    while (this.svgArea.firstChild) {
        this.svgArea.firstChild.remove();
    }

    for (let x = 0; x < plateArray.length; x++) {
        // Header
        const h2 = document.createElement("h3");
        h2.innerText = plateArray[x].serial + " (" + plateArray[x].getWidthWithTrim() + "mm x " + plateArray[x].getLengthWithTrim() + "mm x " + plateArray[x].height + "mm)";
        // SVG
        const svg = document.createElementNS("http://www.w3.org/2000/svg", "svg");
        svg.id = plateArray[x].serial;
        svg.setAttributeNS(null, "viewBox", "0 0 " + (plateArray[x].getWidthWithTrim()) + " " + (plateArray[x].getLengthWithTrim()));

        const width = plateArray[x].width / diffWidth;
        //svg.setAttribute("style", "padding: " + plateArray[x].trim + "px"); // is evectieve pixels ipv scaling dit meot gebeuren bij het plaatsten

        this.svgArea.appendChild(h2);
        this.svgArea.appendChild(svg);
    }
}
function placeProductsOnCanvas(plates, products) {
    let svg = document.getElementsByTagName("svg");

    // testing
    plates[0].setProduct(products[0]);

    // Products List
    for (let i = 0; i < products.length; i++) {

        calculateIfFits(plates[0], products[i]);



        // Product Quantity
        for (let p = 0; p < products[i].quantity; p++) {
            let group = document.createElementNS("http://www.w3.org/2000/svg", "g");

            let prod = document.createElementNS("http://www.w3.org/2000/svg", "rect");
            prod.setAttribute("width", products[i].width);
            prod.setAttribute("height", products[i].length);
            prod.setAttribute("x", 0);
            prod.setAttribute("y", 0);

            let text = document.createElementNS("http://www.w3.org/2000/svg", "text");
            text.textContent = products[i].info;
            text.setAttribute("x", 20);
            text.setAttribute("y", 50);
            text.setAttribute("font-family", "Verdana");
            text.setAttribute("font-size", 35);
            text.setAttribute("fill", "blue");

            group.append(prod);
            group.append(text);

            svg[0].appendChild(group);

            this.response.innerHTML += "Plate: " + plates[0].getAreaWithTrim() + " mm² | Product: " + products[i].totalArea + " mm² = " + (plates[0].getAreaWithTrim() - products[i].totalArea) + " mm² <br>";
        }
    }

}

// Submit calculator form
function submit() {
    const form = document.getElementById("calculatorData");
    this.saw = createSaw();
    this.plates = createPlates();
    this.products = createProducts();

    createCanvas(this.plates);

    placeProductsOnCanvas(this.plates, this.products);
}