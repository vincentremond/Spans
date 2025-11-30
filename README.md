# `Span<T>` et `Memory<T>`

---

# 🚀 C# Performance

## Optimisation Extrême

Passage de `string` à `Span<T>` ou `Memory<T>`

---

## 🐢 Le Problème : le type `string` n'est pas modifiable

En C#, les chaînes sont **immuables**.
Chaque fois que vous faites :
* `text.Substring(...)`
* `text.Split(...)`
* `str1 + str2`

⚠️ Vous créez une **copie** et une **nouvelle allocation**.
➡️ Le **Garbage Collector** doit nettoyer derrière vous.

---

## ⚡ La Solution : `Span<T>` ou `Memory<T>`

C'est une révolution introduite dans les versions récentes de .NET.

* 🔍 C'est une **fenêtre** (une vue) sur la mémoire.
* 🚫 **Zéro Copie** des données.
* 🚫 **Zéro Allocation** sur le tas (Heap).

On manipule la mémoire existante sans jamais la dupliquer.

---

## 🎯 Objectif du Live

Nous allons créer un **Parser de XML**.

1. 🟢 Coder une version **Naïve** (la façon "classique").
2. 🔴 Coder une version **Optimisée** (avec `Span<T>`).
3. 📊 **BenchmarkDotNet** : Le moment de vérité.

*Objectif : Diviser le temps d'exécution et tuer les allocations.*

---

# 👨‍💻 Place au Code !
