document.querySelector('.custom-file-input').addEventListener('change', function (e) {
    var fileName = document.getElementById("input-file").files[0].name;
    var nextSibling = e.target.nextElementSibling
    nextSibling.innerText = fileName
})

$('#PhotoUpload').on('change', function () {
    var maxsize = 15 * 1024 * 1024;// 15M 
    var spanId = "imgErr";
    var image = 'PhotoUpload';
    var img = document.getElementById(image).value;
    var dot = img.lastIndexOf(".") + 1;
    var extFile = img.substr(dot, img.length).toLowerCase();
    var imgSize = document.getElementById(image);

    if (imgSize.files[0].size > maxsize) {

        document.getElementById(spanId).innerHTML = '檔案過大';
        document.getElementById("phto").remove();
        img = "";
        return;
    }

    if (!(extFile == "jpg" || extFile == "jpeg" || extFile == "png" || extFile == "tiff" || extFile == "icon" || extFile == "gif")) {
        document.getElementById(spanId).innerHTML = '檔案格式錯誤';
        document.getElementById("phto").remove();
        img = "";
        return;
    }


    var fileNumber = $(this).prop('files').length;
    var fileNameTotal = '';
    fileNameTotal += '<div id="phto" class="attachmentsArea mt-2 mb-3">';
    fileNameTotal += '<ul>';
    for (var i = 0; i < fileNumber; i++) {
        var fileName = $(this).prop('files')[i].name;
        //fileNameTotal = fileNameTotal+ "<br />" + fileName+",";
        fileNameTotal += '<li>';
        fileNameTotal += '<div class="d-flex justify-content-between">';
        fileNameTotal += '<div>';
        fileNameTotal += '<i class="fas fa-image fa-lg mgR5"></i>' + fileName;
        fileNameTotal += '</div>';
        fileNameTotal += '<div>';
        // fileNameTotal +='<a class="btn-noStyle deleteAttachmentBTN" title="刪除"><i class="fas fa-times font-red"></i></a>';
        fileNameTotal += '</div>';
        fileNameTotal += '</div>';
        fileNameTotal += '</li>';
    }
    fileNameTotal += '</ul>';
    fileNameTotal += '</div>';
    //$('#photoLabel').text(fileNameTotal);
    $('#abc').html(fileNameTotal.slice(0, -1));
});