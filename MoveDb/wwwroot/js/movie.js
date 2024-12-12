
/** 
 * @typedef {function(object): void } processExternalContent
 * @property {object} results
 */
export function processExternalContent(results) {
    const externalCont = document.querySelector(".external-content");
    if (externalCont) {
        //const videoLink = results.url;
        const videoLink = "";
        const videoElement = (document.createElement("iframe")); // Correct HTML element

        videoElement.src = videoLink;
        videoElement.controls = true; // Uncomment to show video controls
        videoElement.style.width = "90%"; // Add 'px' for CSS style
        videoElement.style.height = "700px"; // Add 'px' for CSS style
        videoElement.autoplay = true; // Correct autoplay property
        videoElement.style.display = "block";
        videoElement.style.border = "1px solid red";
        videoElement.style.position = "relative"

        let listGenre = [];

        results.genres.forEach((result, index) => {
            listGenre.push(result.name)
        })    

        const title = document.createElement("div");
        title.className = "title";
        title.textContent = results.name;
        title.style.position = "relative";
        title.style.color = "#59b4a5";
        title.style.fontSize = "40px";
        title.style.lineHeight = "50px";
        title.style.marginLeft = "100px";
        title.style.textDecoration = "none";
        title.style.fontWeight = "bolder";
        title.style.alignSelf = "self-start";
        title.innerHTML =
            `${results.name} <br /> 
            <span style="font-size: 20px; font-weight: normal; color: lightgray;">
            <i style="color:#59b4a5;"">IDMB</i>: ${results.vote_average} 
            <i style="color:#59b4a5;"">Genre</i>: ${listGenre.join(', ')} 
            <i style="color:#59b4a5;"">Release Date</i>: ${new Date(results.release_date).toLocaleDateString("en-US", { month: "long", day: "numeric", year: "numeric" })} 
            <i style="color:#59b4a5;"">Runtime</i>: ${results.runtime}min</span>`;
 
        
        externalCont.appendChild(videoElement);
        externalCont.appendChild(title)
    }
}
