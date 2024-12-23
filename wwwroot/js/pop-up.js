// Pop-up'ı açma fonksiyonu
function openPopup() {
  document.getElementById("pop-up").style.display = "flex"; // Pop-up'ı görünür yap
}

// Pop-up'ı kapama fonksiyonu (Hayır butonuna basıldığında)
function closePopup() {
  document.getElementById("pop-up").style.display = "none"; // Pop-up'ı gizle
}

function deleteAccount(userId) {
  fetch("/Profile/Delete/" + userId, {
    method: "DELETE",
  })
    .then((response) => {
      if (response.ok) {
        window.location.href = "/Home/Index";
      } else {
        alert("Kullanıcı silinirken bir hata oluştu.");
      }
    })
    .catch((error) => {
      alert("Bir hata oluştu: " + error);
    });
}
