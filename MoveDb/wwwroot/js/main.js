
export const sidebar = document.querySelector(".sidebar");
export const sidebarToggler = document.querySelector(".sidebar-toggler");

/**
 * @typedef {function{object} : object} dynamicBannerContent
 * @property {object} elm
 */
export function dynamicBannerContent(elm) {
    // Adjust margin based on sidebar state
    elm.style.marginLeft = sidebar.classList.contains("collapsed") ? "350px" : "350px";
    document.body.appendChild(elm);
}

const updateHyperlinkMargin = () => {
    const linkElement = document.querySelector(".dynamic-link");

    if (linkElement)
        linkElement.style.marginLeft = sidebar.classList.contains("collapsed") ? "350px" : "200px";
};

// Function to update the description margin dynamically
const updateDescriptionMargin = () => {
    const descriptionElement = document.querySelector(".dynamic-description")

    if (descriptionElement)
        descriptionElement.style.marginLeft = sidebar.classList.contains("collapsed") ? "350px" : "200px";
};

const updateTabsElementMargin = () => {
    const tabsElm = document.querySelector(".dynamic-tabs")
    const dynamicLinkWidth = document.querySelector(".dynamic-link").getBoundingClientRect().width;
    if (tabsElm) {
        tabsElm.style.left = sidebar.classList.contains("collapsed") ? `${dynamicLinkWidth + 10}px` : `${dynamicLinkWidth - 130}px`;
    }
}

// Add listener to sidebar toggle for real-time adjustment
if (sidebarToggler) {
    sidebarToggler.addEventListener("click", () => {
        updateHyperlinkMargin()
        updateDescriptionMargin()
        updateTabsElementMargin()
    });
}