$(document).ready(
    function () {
       
        setTimeout(
            
            function () {
                $("#home").fadeOut("slow");
                $("#mainPg").show();
            },

            3000
        )
        oriP3ObjMt = removePX($("#dvP3Obj").css("margin-top"));
        $("#btnFlush").click(function () { window.location.reload(); });
        //$("#dvP3Obj").mousedown(p3ObjDown);
        //$(document).mousemove(p3ObjMove);
        //$(document).mouseup(p3ObjUp);
        $("#dvP3Obj").on("touchstart", p3ObjDown);
        $("#dvP3Obj").on("touchmove", p3ObjMove);
        $("#dvP3Obj").on("touchend", p3ObjUp);
        //$("#dvP3Obj").on("touchend", function (e) { e.offsetY });
    }
    
);
var oriP3ObjMt;
var isP3ObjDown = false;
var oriY;
var index;
var type;
function titleClick(tp) {
    var light = $("#light");
    var flash = $("#flash");
    var tree = $("#tree");
    var mainPg = $("#mainPg");
    var lightCtnt = $("#ctntLight");
    var flashCtnt = $("#ctntFlash");
    var treeCtnt = $("#ctntTree");
    switch (tp) {
        case "light": {
            if (light.attr("class") == "b5") {
                return;
            }
            light.addClass("b5");
            flash.removeClass("b5");
            tree.removeClass("b5");
            mainPg.attr("index", "0");
            lightCtnt.fadeIn("fast");
            flashCtnt.hide();
            treeCtnt.hide();
        }; break;
        case "flash": {
            if (flash.attr("class") == "b5")
                return;
            light.removeClass("b5");
            flash.addClass("b5");
            tree.removeClass("b5");
            mainPg.attr("index", "1");
            lightCtnt.hide();
            flashCtnt.fadeIn("fast");
            treeCtnt.hide();
        }; break;
        case "tree": {
            if (tree.attr("class") == "b5")
                return;
            light.removeClass("b5");
            flash.removeClass("b5");
            tree.addClass("b5");
            mainPg.attr("index", "2");
            lightCtnt.hide();
            flashCtnt.hide();
            treeCtnt.fadeIn("fast");
        }; break;
    }
}
function onNext() {
    var titles = ["light", "flash", "tree"];
    var nextIdx = parseInt( $("#mainPg").attr("index")) + 1;
    if (nextIdx >= titles.length)
        return;
    titleClick(titles[nextIdx]);
}
function onPrevious() {
    var titles = ["light", "flash", "tree"];
    var nextIdx = parseInt( $("#mainPg").attr("index")) - 1;
    if (nextIdx < 0)
        return;
    titleClick(titles[nextIdx]);
}
function p2Ok() {
    var msg = $("#txtMsg").val();
    if (msg == "") {
        alert("还是写一写东西吧");
        return;
    }
    if (msg.length > 50) {
        alert("字数控制在50字以下");
        return;
    }
    onShowP3();
}
function p2Cancel() {
    $(".p2").slideUp();
    $(".b1").show();
    $(".b2").hide();
}
function onShowP2(idx, tp) {
    index = idx;
    type = tp;
    var imgSrc;
    switch (tp) {
        case "light": imgSrc = "images/deng" + (idx + 1) + ".png"; break;
        case "flash": imgSrc = "images/yanhua" + (idx + 1) + ".png"; break;
        case "tree": imgSrc = "images/shu2.png"; break;
    }
    $("#imgP2Obj").attr("src", imgSrc);
    $(".p2").slideDown();
    $(".b1").hide();
    $(".b2").show();
}
function onShowP3() {
    let imgSrc = $("#imgP2Obj").attr("src");
    $("#imgP3Obj").attr("src", imgSrc);
    $(".p3").fadeIn();
    $(".p2").hide();
    $(".b2").hide();
    $(".b1").show();
    $("#mainContent").hide();
}
function p3ObjMove(e) {
    console.log("移动");
    if (!isP3ObjDown)
        return;
    //console.log(e);
    let currY = e.targetTouches[0].screenY;
    let offY = currY - oriY;
    if (offY > 0)
        return;
    console.log((oriP3ObjMt + offY) + "px");
    $("#dvP3Obj").css({ "margin-top": (oriP3ObjMt + offY)+"px" });
    //console.log(currY+" "+ currY - oriY);
}
function p3ObjDown(e) {
    console.log(e);
    //console.log(e.targetTouches[0].screenX + " " + e.targetTouches[0].screenY);
    isP3ObjDown = true;
    oriY = e.targetTouches[0].screenY;
    console.log("原始Y：" + oriY);

}
function p3ObjUp(e) {
    console.log("松开");
    isP3ObjDown = false;
    let mt = removePX($("#dvP3Obj").css("margin-top"));
    console.log("Top:" + mt);
    if (mt > -100) {
        $("#dvP3Obj").css({ "margin-top": oriP3ObjMt+"px" });
        return;
    }
    alert("恭喜，您的祝福已发送");
    window.location.reload();
}
function removePX(oriStr) {
    return parseInt( oriStr.substr(0, oriStr.length - 2));
}
function send() {
    let url = document.location.origin + "/API";
    let data = { "index": index, "type": type };
    $.post(url,  data, function (ret) {
        console.log(ret);
    });
}