// =============================== //
// == Start of the carousel js == //
// ============================= //

import { dynamicBannerContent } from './main.js';

/**
 * @typedef {function(object[]) : object[]} processResult
 * @property {object[]} results
 * @returns {object[]}
 */
export function processResult(results) {

    results.forEach((result, index) => {

        const inputSelector = `input#b${index + 1}`;
        const descriptionId = `description-${index + 1}`;
        const inputElement = document.querySelector(inputSelector);

        const styleSheet = document.styleSheets[0];
            styleSheet.insertRule(`
            body:has(input#b${index + 1}:checked), .thumbnails label[for=b${index + 1}] {
                --img: url(${result.backdrop_path});
            }
        `, styleSheet.cssRules.length);

        // Function to create and append the hyperlink
        const createHyperlink = () => {
            // Remove existing dynamic link
            document.querySelectorAll(".dynamic-link").forEach(el => el.remove());

            // Create a new link if the current input is checked
            if (inputElement.checked) {
                const linkElement = document.createElement("a");
                linkElement.textContent = result.name;
                linkElement.href = `${result.id}/watch`;
                linkElement.style.position = "absolute";
                linkElement.style.top = "50vh";
                linkElement.style.color = "#fff";
                linkElement.style.fontSize = "40px";
                linkElement.style.lineHeight = "45px";
                linkElement.style.textDecoration = "none";
                linkElement.style.fontWeight = "bolder";
                linkElement.style.textShadow = "0px 1px 0 #000000d9";
                linkElement.style.whiteSpace = "pre-wrap";
                linkElement.style.transition = "all 0.5s ease 0s";
                linkElement.className = "dynamic-link";

                dynamicBannerContent(linkElement);
            }
        };

        const createTabs = () => {
            document.querySelectorAll(".dynamic-tabs").forEach(j => j.remove());
            const dynamicLinkWidth = document.querySelector(".dynamic-link").getBoundingClientRect().width;

            if (inputElement.checked) {
                const tabsElement = document.createElement("div");
                tabsElement.className = "dynamic-tabs";
                tabsElement.textContent = `IMDB: ${result.vote_average} Genres: ${result.genres.join(", ")}`;
                tabsElement.style.position = "absolute";
                tabsElement.style.top = "51vh";
                tabsElement.style.left = `${dynamicLinkWidth + 10}px`;
                tabsElement.style.color = "#fff";
                tabsElement.style.fontSize = "20px";
                tabsElement.style.lineHeight = "45px";
                tabsElement.style.textDecoration = "none";
                tabsElement.style.textShadow = "0px 1px 0 #000000d9";
                tabsElement.style.whiteSpace = "pre-wrap";
                tabsElement.style.transition = "all 0.5s ease 0s";

                dynamicBannerContent(tabsElement)
            }
        }

        // Function to create/update the description
        const createDescription = () => {
            // Remove existing descriptions
            document.querySelectorAll(".dynamic-description").forEach(el => el.remove());

            // Create a new description if the current input is checked
            if (inputElement.checked) {
                const descriptionElement = document.createElement("div");
                descriptionElement.textContent = result.overview;
                descriptionElement.id = descriptionId;
                descriptionElement.className = "dynamic-description";
                descriptionElement.style.position = "absolute";
                descriptionElement.style.top = "55vh";
                descriptionElement.style.color = "black";
                descriptionElement.style.fontSize = "15px";
                descriptionElement.style.whiteSpace = "pre-wrap";
                descriptionElement.style.height = "8%";
                descriptionElement.style.padding = "1vmin";
                descriptionElement.style.textAlign = "left";
                descriptionElement.style.marginRight = "250px";
                descriptionElement.style.opacity = "0.8";
                descriptionElement.style.zIndex = "1";
                descriptionElement.style.transition = "all 0.5s ease";
                descriptionElement.style.background = "#808080";

                // Adjust margin-left based on sidebar state
                descriptionElement.style.marginLeft = "350px";
                dynamicBannerContent(descriptionElement)
            }
        };

        if (inputElement) {
            // Add event listener for dynamic changes
            inputElement.addEventListener("change", createDescription);
            inputElement.addEventListener("change", createHyperlink);
            inputElement.addEventListener("change", () => {
                document.querySelectorAll(".dynamic-tabs").forEach(j => j.remove());
                createTabs();
            });

            // Load the hyperlink during page load
            if (inputElement.checked) {
                createDescription();
                createHyperlink();
                createTabs();
            }
        } else {
            console.error(`Input element with ID "b${index + 1}" not found.`);
        }
    });

    return results;
}
