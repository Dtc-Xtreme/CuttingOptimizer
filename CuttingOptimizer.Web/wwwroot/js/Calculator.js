window.addEventListener('DOMContentLoaded', function () {
    let dataUrl = 'https://localhost:44308/json/data.json'

    let productData;

    let canvas = d3.select("#canvas");

    let drawTreeMap = () => {
        let hierarchy = d3.hierarchy(productData, (node) => {
            return node['children']
        }).sum((node) => {
            return node['value']
        });

        let createTreeMap = d3.treemap().size([1000, 600]);

        createTreeMap(hierarchy);

        let productTiles = hierarchy.leaves();
        console.log(productTiles);

        let block = canvas.selectAll('g')
            .data(productTiles)
            .enter()
            .append('g')
            .attr('transform', (movie) => {
                return 'translate(' + movie['x0'] + ', ' + movie['y0'] + ')';
            })

        block.append('rect')
            .attr('class', 'tile')
            .attr('width', (movie) => {
                return movie['x1'] - movie['x0'];
            })
            .attr('height', (movie) => {
                return movie['y1'] - movie['y0'];
            })

    }

    d3.json(dataUrl).then(
        (data, error) => {
            if (error) {
                console.log(error);
            } else {
                productData = data;
                console.log(data);
                drawTreeMap();
            }
        }
    )
})

// Classes
class Saw {
    constructor(serial, thickness) {
        this.serial = serial;
        this.thickness = thickness;
    }
}

class Plate {
    constructor(quantity, serial, width, length, height, vineer) {
        this.quantity = quantity;
        this.serial = serial;
        this.width = width;
        this.length = length;
        this.height = height;
        this.vineer = vineer;
    }
}

class Product {
    constructor(quantity, width, length, height, info) {
        this.quantity = quantity;
        this.width = width;
        this.length = length;
        this.height = height;
        this.info = info;
    }
}

const svgArea = document.getElementById("svgArea");
const canvasArea = document.getElementById("canvasArea");

// Submit calculator form
function submit() {
    let form = document.getElementById("calculatorData");

    const saw = createSaw()
    const plates = createPlates();
    const products = createProducts();

    createCanvas(plates);

    placeProductsOnCanvas(products)
}

function createSaw(input) {
    let saw = new Saw(document.getElementById("Saw_ID").value, document.getElementById("Saw_Thickness").value)
    return saw;
}

function createPlates() {
    let plates = [];

    let platesInput = document.querySelectorAll('[id^="Plates_"');

    let counter = platesInput.length / 6;
    for (let x = 0; x < platesInput.length; x = x + 6) {
        //console.log(x);
        plates.push(new Plate(platesInput[x].value, platesInput[x + 1].value, platesInput[x + 2].value, platesInput[x + 3].value, platesInput[x + 4].value, platesInput[x + 5].checked));
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
        products.push(new Product(productsInput[x].value, productsInput[x + 1].value, productsInput[x + 2].value, productsInput[x + 3].value, productsInput[x + 4].value));
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
        this.svgArea.firstChild.remove()
    }

    for (let x = 0; x < plateArray.length; x++) {
        // Header
        const h2 = document.createElement("h3");
        h2.innerText = plateArray[x].serial;
        // SVG
        const svg = document.createElementNS("http://www.w3.org/2000/svg", "svg");
        svg.id = plateArray[x].serial;
        svg.setAttributeNS(null, "viewBox", "0 0 " + plateArray[x].width + " " + plateArray[x].length);

        const width = plateArray[x].width / diffWidth;
        svg.style = "width: " + width + "%";

        this.svgArea.appendChild(h2);
        this.svgArea.appendChild(svg);
    }
}

function placeProductsOnCanvas(products) {
    const svg = document.getElementsByTagName("svg");

    let prod1 = document.createElementNS("http://www.w3.org/2000/svg", "rect");
    prod1.setAttribute("width", products[0].width);
    prod1.setAttribute("height", products[0].length);
    prod1.setAttribute("x", 0);
    prod1.setAttribute("y", 0);

    let prod2 = document.createElementNS("http://www.w3.org/2000/svg", "rect");
    prod2.setAttribute("width", products[0].width);
    prod2.setAttribute("height", products[0].length);
    prod2.setAttribute("x", 0);
    prod2.setAttribute("y", 0);

    svg[0].appendChild(prod1);
    svg[1].appendChild(prod2);
    //console.log(svg);
}