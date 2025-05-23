<?php

try {
    $dsn = "mysql:host=sql7.freesqldatabase.com;dbname=sql7780794";
    $username = "sql7780794";
    $password = "WZ2vk8QA66";
    $id = $_POST["id"];
    $score = $_POST["score"];
    $pdo = new PDO($dsn, $username, $password);
    $pdo->setAttribute(PDO::ATTR_ERRMODE, PDO::ERRMODE_EXCEPTION);
    $pdo->setAttribute(PDO::ATTR_DEFAULT_FETCH_MODE, PDO::FETCH_ASSOC);

    $stmt = $pdo->prepare('UPDATE Users SET score = :score WHERE id = :id');
    $stmt->execute([':score' => $score, ':id'=> $id]);



} catch (PDOException $e) {
    echo "Erreur PDO : " . $e->getMessage();
}
?>
