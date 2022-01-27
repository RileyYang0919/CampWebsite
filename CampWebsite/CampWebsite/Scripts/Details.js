////let preY = document.querySelector("#pre_year");
////let nextY = document.querySelector("#next_year");
////let date = document.querySelector(".date");

////preY.addEventListener("click", function () {
////    t_year.innerHTML = parseInt(t_year.innerHTML) - 1;
////})
////nextY.addEventListener("click", function () {
////    t_year.innerHTML = parseInt(t_year.innerHTML) + 1;
////})
//////取得當月第一天是星期幾
////function getDay() {
////    let t_year = document.querySelector("#t_year");
////    let month = document.querySelector("#month");
////    let date = new Date(parseInt(t_year.innerHTML), parseInt(month.value), 0);
////    let first_day = date.getDay() + 1;
////    if (date.getDay() == 8) {
////        first_day = 1;
////    }
////    return first_day;
////}
//////取得當月最後一天是星期幾
////function getLastDay() {
////    let t_year = document.querySelector("#t_year");
////    let month = document.querySelector("#month");
////    let date = new Date(parseInt(t_year.innerHTML), parseInt(month.value) + 1, 0);
////    let Last_day = date.getDay();
////    if (date.getDay() == 8) {
////        Last_day = 1;
////    }
////    return Last_day;
////}
////getLastDay()
//////取得上月最後一天是幾日
////function getPreDate() {
////    let t_year = document.querySelector("#t_year");
////    let month = document.querySelector("#month");
////    let date = new Date(parseInt(t_year.innerHTML), parseInt(month.value), 0);
////    return date.getDate();
////}

//////取得當月有幾天
////function getDate() {
////    let t_year = document.querySelector("#t_year");
////    let month = document.querySelector("#month");
////    let date = new Date(parseInt(t_year.innerHTML), parseInt(month.value) + 1, 0);
////    return date.getDate();
////}


//////建立日
////function addDates() {
////    let date_count = 0;
////    document.querySelector(".date").innerHTML = ""; //清除dates
////    let i = 0;
////    let j = 1;
////    let d = getDay();
////    let PreDate = getPreDate() - d + 1;
////    //上月
////    for (i; i < getDay(); i++) {
////        if (getDay() == 7)
////            break;
////        while (d != 0) {
////            date.innerHTML += `<div class="preMonth">${PreDate}</div>`;
////            d--;
////            PreDate++;
////            date_count++;
////        }

////    }
////    //當月
////    for (j; j <= getDate(); j++) {
////        date.innerHTML += `<div>${j}</div>`;
////        date_count++;
////    }
////    //下月
////    let firstDate = 1;
////    while (date_count < 42) {
////        date.innerHTML += `<div class="nextMonth">${firstDate}</div>`;
////        firstDate++;
////        date_count++;
////    }
////    console.log(date_count)
////}
////addDates();
////document.getElementById("month").addEventListener("change", addDates)
////document.querySelector("#t_year").addEventListener("DOMNodeInserted", addDates)
