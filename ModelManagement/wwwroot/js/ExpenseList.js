"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/expenseList").build();

connection.on("NewExpense", function (id, timestamp) {
    var encodedMsg = "Id: " + id + " ,Timestamp: " + timestamp;
    var li = document.createElement("li");
    li.textContent = encodedMsg;
    document.getElementById("messagesList").appendChild(li);
});

connection.start().then(function () {
}).catch(function (err) {
    return console.error(err.toString());
});
