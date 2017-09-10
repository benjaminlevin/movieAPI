
$(document).ready(function(){
    load();
});

function load(){
    $.get(
        "/load",
        (res) => {
            var str = "";
            var i = 0;
            while(i<res.length)
            {
                str += "<h6>";
                str += res[i].title + " | "; 
                str += "Rating: " + res[i].vote_average + " | "; 
                str += "Released: " + res[i].release_date;
                str += "</h6>";
                i++;
            }
            $(".Info").html(str);
        }
    )
    console.log("AJAX: page loaded");
}

$("#search").click(function(e){
    e.preventDefault();
    var search = $('input[name="mSearch"]').val();
    $.post("/search", { SearchVariable: search }, function(result){
        if(result.failed){
            $("#error").html("error");
        }
        else{
            load();
        }
    });
    console.log("AJAX: search completed");
});
