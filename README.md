## Struktura projektu w Unity
### Hierarchia folderów
Poniżej jest przedstawiona hierarchia folderów stosuj się do niej organizując, tworząc i importując assety do Unity.
```python
Assets
├── Audio  #Tutaj znajdują się pliki audio
│   └── SFX  #Tutaj znajdują się efekty dźwiękowe
├── Scenes  #Tutaj znajdują się sceny
├── Scripts  #Tutaj znajdują się skrypty
├── Settings  #Tutaj znajdują się ustawienia (głównie render pipeline'y)
├── StreamingAssets  #Wszystkie umieszczone tu pliki nie wchodzą do builda a są zaciągane dynamicznie
│   └── Music  #Tutaj znajduje się muzyka (ładowana dynamicznie aby zaoszczędzić pamięć gry)
└── Textures - Materials  #Tutaj przechowywane są tekstury i materiały
```
## Clean Code i Clean Project
Konwencja nazewnictwa ma nam pomóc urzymać spójny kod i projekt. Jeśli nie wiesz jak nazwać jakąś zmienną lub plik znajdziesz to poniżej.

### Zmienne
- camelCase
- Zmienne lokalne powinny być opisowe, ale zwięzłe. Powinny unikać skrótów i symboli niezwiązanych z przeznaczeniem.
```csharp
int playerScore = 0;
string levelName = "Tutorial";
```
### Metody
- PascalCase
- Nazwy metod powinny być czasownikami lub frazami czasownikowymi, które jasno opisują ich funkcję.
```csharp
public void SaveGame() { }
public int CalculateDamage() { }
```
### Klasy i Struktury
- PascalCase
- Nazwy klas i struktur powinny być rzeczownikami opisującymi ich przeznaczenie.
```csharp
public class PlayerController { }
public struct DamageInfo { }
```
### Pola
- camelCase
- Pola prywatne powinny być poprzedzone podkreśleniem _. Pól publicznych należy unikać, i używać zamiast nich Właściwości.
```csharp
private int _health;
public int Health { get; private set; }
```
### Właściwości
- PascalCase
- Właściwości zawsze powinny być publiczne, a ich nazwy muszą jasno wskazywać przechowywaną wartość.
```csharp
public int Score { get; set; }
public string PlayerName { get; private set; }
```
### Stałe
- PascalCase
- Stałe powinny mieć jasno określoną nazwę i być oznaczone jako const lub readonly w zależności od wymagań.
```csharp
public const float Gravity = 9.81f;
public static readonly int MaxPlayers = 4;
```
### Interfejsy
- PascalCase z przedrostkiem I
- Interfejsy powinny być nazywane rzeczownikami lub przymiotnikami opisującymi ich przeznaczenie.
```csharp
public interface IDamageable { }
public interface IInteractable { }
```
### Enumy
- PascalCase
- Nazwy powinny dokładnie odzwierciedlać zawartość.
```csharp
public enum WeaponType
{
    BaseballBat,
    Pistol,
    Machete
}
```
### Namespace'y
- PascalCase
- Namespace'y powinny być zgodne z hierarchią projektu. Należy unikać zbyt ogólnych nazw.
```csharp
namespace MyGame.Projectiles { }
```
### Klasy wbudowane Unity
- Komponenty (Component): klasy dziedziczące po MonoBehaviour muszą jasno opisywać swoją rolę w grze.
- ScriptableObject: nazwy powinny kończyć się na Settings, Config, lub innym adekwatnym określeniu.
```csharp
public class PlayerController : MonoBehaviour { }
public class GameSettings : ScriptableObject { }
```
### Nazwy plików
- Prefiks określający przeznaczenie pliku i PascalCase
```python
ScriptName #Bez prefiksu
M_MaterialName
P_PrefabName
T_TextureName
S_SpriteName
A_AnimationName
AC_AnimationControllerName
AUD_AudioName
```
