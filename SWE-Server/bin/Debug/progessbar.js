document.body.onload = initProgressBar;

function initProgressBar()
{
    var progressBarDiv = document.getElementById("progressBarDiv");
    while (progressBarDiv.childNodes.length > 0)
    {
        progressBarDiv.childNodes[0].removeChild();
    }

    var progressBar = document.createElement("progress");
    progressBar.id = "progressBar";
    progressBarDiv.appendChild(progressBar);

    var percentLabel = document.createElement("span");
    percentLabel.id = "percentLabel";
    progressBarDiv.appendChild(percentLabel);

    setTimeout(updateProgressBar, 0);
}

function updateProgressBar()
{
    var xmlhttp = new XMLHttpRequest();
    xmlhttp.open("GET", "index.php?action=Navi&generationProgress", true);
    xmlhttp.onreadystatechange = readyStateChangeProxy(xmlhttp, resultFunction);
    //xmlhttp.setRequestHeader("Content-type", "application/x-www-form-urlencoded; charset=UTF-8");
    xmlhttp.send();
}

function resultFunction(response)
{
    var progressBar = document.getElementById("progressBar");
    progressBar.value = response.progress;
    var percentLabel = document.getElementById("percentLabel");
    percentLabel.innerHTML = (response.progress * 100).toFixed(0) + "%";

    if (response.progress > .999)
        window.location = "?action=Navi";
    else
        setTimeout(updateProgressBar, 2000);
}

function readyStateChangeProxy(ajaxObject, resultFunction)
{
    return function()
    {
        if (ajaxObject.readyState == 4 && ajaxObject.status == 200)
        {
            var result;
            if (ajaxObject.responseText.length > 0)
                result = eval("(" + ajaxObject.responseText + ")");
            resultFunction(result);
        }
        else if (ajaxObject.readyState == 4 && ajaxObject.status != 200)
        {
            alert("HTTP Fehler " + ajaxObject.status + ": " + ajaxObject.responseText);
        }
    }
}