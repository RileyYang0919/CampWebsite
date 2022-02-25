//輪播圖
//上下輪播效果
var img_up = document.querySelector(".img-up");
var img_down = document.querySelector(".img-down");
var img_container = document.querySelector(".img-container");
var img_length = img_container.children.length;
var isMoveDone = true;
const carousel_intertval = 200;
const locationString = `${location.protocol}//${location.host}`;
img_up.addEventListener("click", () => {
    if (!isMoveDone) {
        return;
    }
    isMoveDone = false;
    const src_three = decodeURI(img_container.children[4].src).replace(locationString, "");
    img_container.classList.add("img-transition");
    img_container.style.transform = `translateY(${-116}px)`;
    setTimeout(() => {
        img_container.classList.remove("img-transition")
        let node = document.createElement("img");
        node.src = src_three
        node.className = "mb-md-3 me-2 opacity-25";
        img_container.appendChild(node);
        img_container.children[0].remove();
        img_container.style.transform = `translateY(0px)`;
        isMoveDone = true;
    }, carousel_intertval);
})

img_down.addEventListener("click", () => {
    if (!isMoveDone) {
        return;
    }
    isMoveDone = false;
    const src = decodeURI(img_container.children[img_length - 5].src).replace(locationString, "");
    img_container.classList.add("img-transition");
    img_container.style.transform = `translateY(${116}px)`;
    setTimeout(() => {
        img_container.classList.remove("img-transition")
        const node = document.createElement("img");
        node.src = src;
        node.className = "mb-md-3 me-2 opacity-25";
        img_container.insertBefore(node, img_container.children[0]);
        img_container.children[img_length].remove();
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
    const src_three = decodeURI(img_container.children[4].src).replace(locationString, "");
    img_container.classList.add("img-transition");
    img_container.style.transform = `translateX(-108px)`;
    setTimeout(() => {
        img_container.classList.remove("img-transition")
        const node = document.createElement("img");
        node.src = src_three;
        node.className = "mb-md-3 me-2 opacity-25";
        img_container.appendChild(node);
        img_container.children[0].remove();
        img_container.style.transform = `translateX(0px)`;
        isMoveDone = true;
    }, carousel_intertval);
})
img_right.addEventListener("click", () => {
    if (!isMoveDone) {
        return;
    }
    isMoveDone = false;
    const src = decodeURI(img_container.children[img_length - 5].src).replace(locationString, "");
    img_container.classList.add("img-transition");
    img_container.style.transform = `translateX(108px)`;
    setTimeout(() => {
        img_container.classList.remove("img-transition")
        const node = document.createElement("img");
        node.src = src
        node.className = "mb-md-3 me-2 opacity-25";
        img_container.insertBefore(node, img_container.children[0]);
        img_container.children[img_length].remove()
        img_container.style.transform = `translateX(0px)`;
        isMoveDone = true;
    }, carousel_intertval);
})
//點擊圖片輪播
$(function () {
    console.log(img_container)
    if (img_container.children.length <= 2) {
     
        img_container.children[0].click();//預設點擊第一張

    }
    else {
        img_container.children[2].click();
    }
})
var img_container_img = img_container.querySelectorAll("img");
var img_selected = document.querySelector("#img_selected")
img_container.addEventListener("click", (e) => {
    if (e.target.src != null) {
        for (let i = 0; i < img_container.children.length; i++) {
            img_container.children[i].classList.add("opacity-25");
        }
        img_selected.src = decodeURI(e.target.src).replace(locationString, "");
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


