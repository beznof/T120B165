# T120B165

## 1.	Sprendžiamo uždavinio aprašymas
### 1.1.	Sistemos paskirtis
Esminis kuriamos sistemos tikslas – suteikti užsienio kalbų žodžių ar frazių bei jų vertimų talpyklą mokytojams, mokiniams ir kalbų entuziastams.

Sistemoje koreliuos trys esybės, susietos hierarchiniu ryšiu, einančiu žemyn:
•	Kalba
•	Žodynas
•	Žodyno įrašai (žodžiai ar frazės)

Bendrai apie naudojimo scenarijų:
Sistemoje administratoriai kurs realias ar fikcines kalbas, prie kurių mokytojai galės kurti žodynus susidarančius iš žodžių ar frazių, pateikiant jų vertimus. Besimokantieji galės šias kalbas, jų žodynus ir atitinkamus įrašus peržiūrėti bei išsisaugoti.

### 1.2.	Funkciniai reikalavimai
Toliau pateikti funkciniai reikalavimai, sugrupuoti pagal naudotojų tipus:
-	Neregistruotas naudotojas galės:
    -	Peržiūrėti kalbas, žodynus ir jų įrašus, pagal poreikį papildomai naudojant paiešką ir filtravimą.
    -	Registruoti mokytojo tipo paskyrą.
    -	Registruoti besimokančiojo tipo paskyrą.
    -	Prisijungti prie paskyros.
-	Besimokantieji galės:
    -	Peržiūrėti kalbas, žodynus ir jų įrašus, pagal poreikį papildomai naudojant paiešką ir filtravimą.
    -	Išsisaugoti žodynus bei peržiūrėti ir šalinti išsaugotus.
    -	Pasižymėti įrašus žodyne bei peržiūrėti ir šalinti pažymėtus.
    -	Atsijungti nuo sistemos.
    -	Mokytojas galės:
    -	Peržiūrėti kalbas, žodynus ir jų įrašus.
    -	Atskirai peržiūrėti savo sukurtus žodynus.
    -	Prie kalbų kurti ir valdyti savo žodynus, nurodant pavadinimą, nuotrauką ir kalbos lygį.
    -	Prie savo žodyno kurti ir valdyti įrašus, pateikiant žodį ar frazę, vertimą, tarimo transkripciją, bei sinonimus.
    -	Atsijungti nuo sistemos.
-	Administratorius galės:
    -	Peržiūrėti kalbas, žodynus ir jų įrašus.
    -	Kurti ir valdyti kalbas, nurodant kalbos pavadinimą, nuotrauką ir šeimą.
    -	Valdyti žodynus ir jų įrašus.
    -	Valdyti naudotojų paskyras.
    -	Tvirtinti mokytojo tipo paskyrų registracijas.
    -	Atsijungti nuo sistemos.

## 2.	Sistemos architektūra
Sistemą sudarys keturi esminiai komponentai:
1.	Kliento pusė – implementuojama React (TypeScript) karkasu.
2.	Serverio pusė – implementuojama ASP.NET karkasu.
3.	Objektų saugykla.
4.	Duomenų bazė – implementuojama MySQL duomenų bazės varikliu.

Sistema bus diegiama Microsoft Azure debesijos paslaugų platformoje. Kliento pusė bus diegiama Azure Static Web Apps. Serverio pusė bus konteinerizuota (konteineriai talpinami Azure Container Registry) ir diegiama Azure Container Apps. Su jais HTTPS protokolu bendraus kliento įrenginys. Nuotraukos (kitaip objektai) bus talpinami Azure Blob Storage. Pastarąjį kliento įrenginys ir serverio pusė pasieks taip pat HTTPS protokolu. Duomenų bazė bus realizuojama pasitelkiant Azure Database for MySQL, su kuria serverio pusė bendraus TCP protokolu.
Toliau pateikta tiksli diegimo diagrama:
 
<img width="926" height="653" alt="image" src="https://github.com/user-attachments/assets/ba844ade-270b-48fa-9eeb-fce0994813fd" />
