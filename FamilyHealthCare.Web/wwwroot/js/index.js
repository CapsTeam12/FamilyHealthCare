document.querySelector("#login").onclick = function (event) {
  event.preventDefault();
  document.getElementById("login").style.display = "none";
  document.getElementById("user-success").style.display = "block";
};
