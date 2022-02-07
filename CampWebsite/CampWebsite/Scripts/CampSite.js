//輪播圖
//上下輪播效果
var img_up = document.querySelector(".img-up");
var img_down = document.querySelector(".img-down");
var img_container = document.querySelector(".img-container");
var img_length = img_container.childNodes.length;
var isMoveDone = true;
const carousel_intertval = 200;

img_up.addEventListener("click", () => {
    if (!isMoveDone) {
        return;
    }
    isMoveDone = false;
    const src_three = img_container.childNodes[9].src;
    img_container.classList.add("img-transition");
    img_container.style.transform = `translateY(${-116}px)`;
    setTimeout(() => {
        img_container.classList.remove("img-transition")
        const node_space = document.createElement("div")
        const node = document.createElement("img");
        node.src = src_three;
        node.className = "mb-md-3 me-2 opacity-25";
        img_container.appendChild(node);
        img_container.appendChild(node_space);
        img_container.childNodes[1].remove();
        img_container.childNodes[0].remove();
        img_container.style.transform = `translateY(0px)`;
        isMoveDone = true;
    }, carousel_intertval);
})

img_down.addEventListener("click", () => {
    if (!isMoveDone) {
        return;
    }
    isMoveDone = false;
    const src = img_container.childNodes[img_length - 10].src;
    img_container.classList.add("img-transition");
    img_container.style.transform = `translateY(${116}px)`;
    setTimeout(() => {
        img_container.classList.remove("img-transition")
        const node_space = document.createElement("div")
        const node = document.createElement("img");
        node.src = src;
        node.className = "mb-md-3 me-2 opacity-25";
        img_container.insertBefore(node, img_container.childNodes[0]);
        img_container.insertBefore(node_space, img_container.childNodes[0]);
        img_container.childNodes[img_length - 1].remove();
        img_container.childNodes[img_length - 1].remove();
        img_container.style.transform = `translateY(0px)`;
        isMoveDone = true;
    }, carousel_intertval);
})
// 左右輪播效果
var img_left = document.querySelector(".img-left");
var img_right = document.querySelector(".img-right");

img_left.addEventListener("click", () => {
    if (!isMoveDone) {
        return;
    }
    isMoveDone = false;
    const src_three = img_container.childNodes[9].src;
    img_container.classList.add("img-transition");
    img_container.style.transform = `translateX(-108px)`;
    setTimeout(() => {
        img_container.classList.remove("img-transition")
        const node_space = document.createElement("div")
        const node = document.createElement("img");
        node.src = src_three;
        node.className = "mb-md-3 me-2 opacity-25";
        img_container.appendChild(node);
        img_container.appendChild(node_space);
        img_container.childNodes[1].remove();
        img_container.childNodes[0].remove();
        img_container.style.transform = `translateX(0px)`;
        isMoveDone = true;
    }, carousel_intertval);
})
img_right.addEventListener("click", () => {
    if (!isMoveDone) {
        return;
    }
    isMoveDone = false;
    const src = img_container.childNodes[img_length - 10].src;
    img_container.classList.add("img-transition");
    img_container.style.transform = `translateX(108px)`;
    setTimeout(() => {
        img_container.classList.remove("img-transition")
        const node_space = document.createElement("div")
        const node = document.createElement("img");
        node.src = src;
        node.className = "mb-md-3 me-2 opacity-25";
        img_container.insertBefore(node, img_container.childNodes[0]);
        img_container.insertBefore(node_space, img_container.childNodes[0]);
        img_container.childNodes[img_length - 1].remove();
        img_container.childNodes[img_length - 1].remove();
        img_container.style.transform = `translateX(0px)`;
        isMoveDone = true;
    }, carousel_intertval);
})
//點擊圖片輪播
var img_container_img = img_container.querySelectorAll("img");
var img_selected = document.querySelector("#img_selected")
img_container.addEventListener("click", (e) => {
    if (e.target.src != null) {
        for (let i = 0; i < img_container.children.length; i++) {
            img_container.children[i].classList.add("opacity-25");
        }
        img_selected.src = e.target.src;
        img_selected.classList.add("show-img-selected");
        e.target.classList.remove("opacity-25");
        setTimeout(() => {
            img_selected.classList.remove("show-img-selected");
        }, 100)
    }
    else {
        return;
    }
})
//輪播圖結束=================================


