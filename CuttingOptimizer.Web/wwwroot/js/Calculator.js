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



    //// set the dimensions and margins of the graph
    //const margin = { top: 10, right: 10, bottom: 10, left: 10 },
    //    width = 445 - margin.left - margin.right,
    //    height = 445 - margin.top - margin.bottom;

    //// append the svg object to the body of the page
    //const svg = d3.select("#my_dataviz")
    //    .append("svg")
    //    .attr("width", width + margin.left + margin.right)
    //    .attr("height", height + margin.top + margin.bottom)
    //    .append("g")
    //    .attr("transform",
    //        `translate(${margin.left}, ${margin.top})`);


    //// read json data
    //d3.json(dataUrl).then(function (data) {
    //    const root = d3.hierarchy(data).sum(function (d) { return d.value }) // Here the size of each leave is given in the 'value' field in input data

    //    // Then d3.treemap computes the position of each element of the hierarchy
    //    let treemap = d3.treemap()
    //        .size([width, height])
    //        .padding(2)
    //        (root);

    //    console.log(treemap);

    //    // use this information to add rectangles:
    //    svg
    //        .selectAll("rect")
    //        .data(root.leaves())
    //        .join("rect")
    //        .attr('x', function (d) { return d.x0; })
    //        .attr('y', function (d) { return d.y0; })
    //        .attr('width', function (d) { return d.x1 - d.x0; })
    //        .attr('height', function (d) { return d.y1 - d.y0; })
    //        .style("stroke", "red")
    //        .style("fill", "slateblue")

    //    // and to add the text labels
    //    svg
    //        .selectAll("text")
    //        .data(root.leaves())
    //        .join("text")
    //        .attr("x", function (d) { return d.x0 + 5 })    // +10 to adjust position (more right)
    //        .attr("y", function (d) { return d.y0 + 20 })    // +20 to adjust position (lower)
    //        .text(function (d) { return d.data.name })
    //        .attr("font-size", "15px")
    //        .attr("fill", "white")
    //})
})