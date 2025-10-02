using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PNC.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Commissariat",
                columns: table => new
                {
                    id = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: false),
                    nom = table.Column<string>(type: "varchar(30)", unicode: false, maxLength: 30, nullable: true),
                    province = table.Column<string>(type: "varchar(30)", unicode: false, maxLength: 30, nullable: true),
                    territoire = table.Column<string>(type: "varchar(30)", unicode: false, maxLength: 30, nullable: true),
                    district = table.Column<string>(type: "varchar(30)", unicode: false, maxLength: 30, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Commissariat_PK", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Enqueteur",
                columns: table => new
                {
                    id = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: false),
                    codeIdentification = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: false),
                    nom = table.Column<string>(type: "varchar(30)", unicode: false, maxLength: 30, nullable: false),
                    postNom = table.Column<string>(type: "varchar(30)", unicode: false, maxLength: 30, nullable: false),
                    prenom = table.Column<string>(type: "varchar(30)", unicode: false, maxLength: 30, nullable: true),
                    telephone = table.Column<string>(type: "varchar(30)", unicode: false, maxLength: 30, nullable: true),
                    adresse = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Enqueteur_PK", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "NUTP",
                columns: table => new
                {
                    id = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: false),
                    numeroNUTP = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: true),
                    status = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NUTP", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Permission",
                columns: table => new
                {
                    id = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: false),
                    nom = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    description = table.Column<string>(type: "varchar(200)", unicode: false, maxLength: 200, nullable: true),
                    module = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    action = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    estActif = table.Column<bool>(type: "bit", nullable: false),
                    dateCreation = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Permission_PK", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "FonctionBefore1997",
                columns: table => new
                {
                    id = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: false),
                    ministere = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    service = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    dernierGrade = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    natureGrade = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    acteNomination = table.Column<string>(type: "varchar(300)", unicode: false, maxLength: 300, nullable: true),
                    matriculeOrigine = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: true),
                    dateEntree = table.Column<DateTime>(type: "datetime", nullable: false),
                    lieu = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false),
                    idCommissariat = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("FonctionBefore1997_PK", x => x.id);
                    table.ForeignKey(
                        name: "FK_Commissariat_FonctionBefore1997",
                        column: x => x.idCommissariat,
                        principalTable: "Commissariat",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "Policier",
                columns: table => new
                {
                    id = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: false),
                    numeroNUTP = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false),
                    matricule = table.Column<string>(type: "varchar(15)", unicode: false, maxLength: 15, nullable: false),
                    nom = table.Column<string>(type: "varchar(30)", unicode: false, maxLength: 30, nullable: false),
                    postNom = table.Column<string>(type: "varchar(30)", unicode: false, maxLength: 30, nullable: true),
                    prenom = table.Column<string>(type: "varchar(30)", unicode: false, maxLength: 30, nullable: true),
                    sexe = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: false),
                    dateNaissance = table.Column<DateTime>(type: "datetime", nullable: false),
                    jour = table.Column<string>(type: "varchar(2)", unicode: false, maxLength: 2, nullable: false),
                    mois = table.Column<string>(type: "varchar(15)", unicode: false, maxLength: 15, nullable: false),
                    annee = table.Column<int>(type: "int", nullable: false),
                    paysNaissance = table.Column<string>(type: "varchar(30)", unicode: false, maxLength: 30, nullable: false),
                    villeNaissance = table.Column<string>(type: "varchar(30)", unicode: false, maxLength: 30, nullable: false),
                    villageNaissance = table.Column<string>(type: "varchar(30)", unicode: false, maxLength: 30, nullable: true),
                    natureGrade = table.Column<string>(type: "varchar(30)", unicode: false, maxLength: 30, nullable: false),
                    acteNominationGrade = table.Column<string>(type: "varchar(300)", unicode: false, maxLength: 300, nullable: true),
                    dateNomination = table.Column<DateTime>(type: "datetime", nullable: true),
                    dateEntreePolice = table.Column<DateTime>(type: "datetime", nullable: false),
                    lieuEntreePolice = table.Column<string>(type: "varchar(30)", unicode: false, maxLength: 30, nullable: false),
                    fonctionActuelle = table.Column<string>(type: "varchar(30)", unicode: false, maxLength: 30, nullable: false),
                    datePriseFonction = table.Column<DateTime>(type: "datetime", nullable: true),
                    uniteMere = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    uniteAffectation = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    nomUnite = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    dept_bn_dist_gpt = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    bu_ciel_ciat_esc_det = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    sec_pi_sciat_sousDet = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    acteNominationFonction = table.Column<string>(type: "varchar(300)", unicode: false, maxLength: 300, nullable: false),
                    dateActe = table.Column<DateTime>(type: "datetime", nullable: true),
                    lieuAffectation = table.Column<string>(type: "varchar(30)", unicode: false, maxLength: 30, nullable: false),
                    etatCivil = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false),
                    groupeSanguin = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: false),
                    rhesus = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: false),
                    provinceOrigine = table.Column<string>(type: "varchar(30)", unicode: false, maxLength: 30, nullable: false),
                    districtOrigine = table.Column<string>(type: "varchar(30)", unicode: false, maxLength: 30, nullable: false),
                    territoireOrigine = table.Column<string>(type: "varchar(30)", unicode: false, maxLength: 30, nullable: false),
                    secteurOrigine = table.Column<string>(type: "varchar(30)", unicode: false, maxLength: 30, nullable: false),
                    villageOrigine = table.Column<string>(type: "varchar(30)", unicode: false, maxLength: 30, nullable: false),
                    numeroRue = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: false),
                    rue = table.Column<string>(type: "varchar(30)", unicode: false, maxLength: 30, nullable: false),
                    commune = table.Column<string>(type: "varchar(30)", unicode: false, maxLength: 30, nullable: false),
                    telephone = table.Column<string>(type: "varchar(30)", unicode: false, maxLength: 30, nullable: false),
                    categoriePermis = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: true),
                    numeroPermis = table.Column<string>(type: "varchar(30)", unicode: false, maxLength: 30, nullable: true),
                    dateDelivrancePermis = table.Column<DateTime>(type: "datetime", nullable: true),
                    photo = table.Column<string>(type: "varchar(300)", unicode: false, maxLength: 300, nullable: false),
                    signature = table.Column<string>(type: "varchar(300)", unicode: false, maxLength: 300, nullable: false),
                    observation = table.Column<string>(type: "varchar(300)", unicode: false, maxLength: 300, nullable: false),
                    idCommissariat = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: true),
                    status = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Policier_PK", x => x.id);
                    table.ForeignKey(
                        name: "FK_Policier_Commissariat",
                        column: x => x.idCommissariat,
                        principalTable: "Commissariat",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "Role",
                columns: table => new
                {
                    id = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: false),
                    nom = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    description = table.Column<string>(type: "varchar(200)", unicode: false, maxLength: 200, nullable: true),
                    estActif = table.Column<bool>(type: "bit", nullable: false),
                    dateCreation = table.Column<DateTime>(type: "datetime", nullable: false),
                    PermissionId = table.Column<string>(type: "varchar(10)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Role_PK", x => x.id);
                    table.ForeignKey(
                        name: "FK_Role_Permission_PermissionId",
                        column: x => x.PermissionId,
                        principalTable: "Permission",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "Conjoint",
                columns: table => new
                {
                    id = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: false),
                    nom = table.Column<string>(type: "varchar(30)", unicode: false, maxLength: 30, nullable: false),
                    postNom = table.Column<string>(type: "varchar(30)", unicode: false, maxLength: 30, nullable: true),
                    prenom = table.Column<string>(type: "varchar(30)", unicode: false, maxLength: 30, nullable: true),
                    nationalite = table.Column<string>(type: "varchar(30)", unicode: false, maxLength: 30, nullable: false),
                    profession = table.Column<string>(type: "varchar(30)", unicode: false, maxLength: 30, nullable: false),
                    matricule = table.Column<string>(type: "varchar(30)", unicode: false, maxLength: 30, nullable: true),
                    dateMariage = table.Column<DateTime>(type: "datetime", nullable: true),
                    idPolicier = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Conjoint_PK", x => x.id);
                    table.ForeignKey(
                        name: "FK_Policier_Conjoint",
                        column: x => x.idPolicier,
                        principalTable: "Policier",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "DistinctionHonorifique",
                columns: table => new
                {
                    id = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: false),
                    dateDecision = table.Column<DateTime>(type: "datetime", nullable: false),
                    numeroDecision = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false),
                    motif = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false),
                    idPolicier = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("DistinctionHonorifique_PK", x => x.id);
                    table.ForeignKey(
                        name: "FK_Policier_DistinctionHonorifique",
                        column: x => x.idPolicier,
                        principalTable: "Policier",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "Document",
                columns: table => new
                {
                    id = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: false),
                    idPolicier = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: false),
                    urlDocument = table.Column<string>(type: "varchar(500)", unicode: false, maxLength: 500, nullable: false),
                    label = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false),
                    description = table.Column<string>(type: "varchar(200)", unicode: false, maxLength: 200, nullable: true),
                    dateCreation = table.Column<DateTime>(type: "datetime", nullable: false),
                    dateModification = table.Column<DateTime>(type: "datetime", nullable: true),
                    typeDocument = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    extension = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: true),
                    tailleFichier = table.Column<long>(type: "bigint", nullable: true),
                    estActif = table.Column<bool>(type: "bit", nullable: false),
                    status = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Document_PK", x => x.id);
                    table.ForeignKey(
                        name: "FK_Document_Policier",
                        column: x => x.idPolicier,
                        principalTable: "Policier",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Empreinte",
                columns: table => new
                {
                    id = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false),
                    idPolicier = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: false),
                    typeDoigt = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Urlepreinte = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Empreint__3213E83FA6E4BA03", x => x.id);
                    table.ForeignKey(
                        name: "FK__Empreinte__idPol__5812160E",
                        column: x => x.idPolicier,
                        principalTable: "Policier",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "Enfant",
                columns: table => new
                {
                    id = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: false),
                    nom = table.Column<string>(type: "varchar(30)", unicode: false, maxLength: 30, nullable: false),
                    postNom = table.Column<string>(type: "varchar(30)", unicode: false, maxLength: 30, nullable: true),
                    prenom = table.Column<string>(type: "varchar(30)", unicode: false, maxLength: 30, nullable: true),
                    dateNaissance = table.Column<DateTime>(type: "datetime", nullable: false),
                    paysNaissance = table.Column<string>(type: "varchar(30)", unicode: false, maxLength: 30, nullable: false),
                    villeNaissance = table.Column<string>(type: "varchar(30)", unicode: false, maxLength: 30, nullable: false),
                    idPolicier = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Enfant_PK", x => x.id);
                    table.ForeignKey(
                        name: "FK_Policier_Enfant",
                        column: x => x.idPolicier,
                        principalTable: "Policier",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "Formation",
                columns: table => new
                {
                    id = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: false),
                    typeFormation = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false),
                    ecole = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    pays = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    ville = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    annee = table.Column<int>(type: "int", nullable: false),
                    diplome = table.Column<string>(type: "varchar(300)", unicode: false, maxLength: 300, nullable: false),
                    nomDiplome = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    duree = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false),
                    nature = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false),
                    idPolicier = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Formation_PK", x => x.id);
                    table.ForeignKey(
                        name: "FK_Policier_Formation",
                        column: x => x.idPolicier,
                        principalTable: "Policier",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "FRI",
                columns: table => new
                {
                    id = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: false),
                    dateRemplissage = table.Column<DateTime>(type: "datetime", nullable: false),
                    jour = table.Column<int>(type: "int", nullable: false),
                    mois = table.Column<string>(type: "varchar(15)", unicode: false, maxLength: 15, nullable: false),
                    annee = table.Column<int>(type: "int", nullable: false),
                    numero = table.Column<string>(type: "varchar(4)", unicode: false, maxLength: 4, nullable: false),
                    idEnqueteur = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: false),
                    idPolicier = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: false),
                    idCommissariat = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("FRI_PK", x => x.id);
                    table.ForeignKey(
                        name: "FK_Commissariat_FRI",
                        column: x => x.idCommissariat,
                        principalTable: "Commissariat",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_FRI_Enqueteur",
                        column: x => x.idEnqueteur,
                        principalTable: "Enqueteur",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_Policier_FRI",
                        column: x => x.idPolicier,
                        principalTable: "Policier",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "HistAffectation",
                columns: table => new
                {
                    id = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: false),
                    lieu = table.Column<string>(type: "varchar(30)", unicode: false, maxLength: 30, nullable: false),
                    denomination = table.Column<string>(type: "varchar(30)", unicode: false, maxLength: 30, nullable: false),
                    acteDenomination = table.Column<string>(type: "varchar(300)", unicode: false, maxLength: 300, nullable: false),
                    dateActe = table.Column<DateTime>(type: "datetime", nullable: false),
                    idPolicier = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("HistAffectation_PK", x => x.id);
                    table.ForeignKey(
                        name: "FK_Policier_HistAffectation",
                        column: x => x.idPolicier,
                        principalTable: "Policier",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "HistFonction",
                columns: table => new
                {
                    id = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: false),
                    intituleFonction = table.Column<string>(type: "varchar(30)", unicode: false, maxLength: 30, nullable: false),
                    datePriseFonction = table.Column<DateTime>(type: "datetime", nullable: false),
                    dateFin = table.Column<DateTime>(type: "datetime", nullable: false),
                    idPolicier = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("HistFonction_PK", x => x.id);
                    table.ForeignKey(
                        name: "FK_Policier_HistFonction",
                        column: x => x.idPolicier,
                        principalTable: "Policier",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "HistGrade",
                columns: table => new
                {
                    id = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: false),
                    intitule = table.Column<string>(type: "varchar(30)", unicode: false, maxLength: 30, nullable: false),
                    acteNomination = table.Column<string>(type: "varchar(300)", unicode: false, maxLength: 300, nullable: false),
                    dateNomination = table.Column<DateTime>(type: "datetime", nullable: false),
                    dateActe = table.Column<DateTime>(type: "datetime", nullable: false),
                    idPolicier = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("HistGrade_PK", x => x.id);
                    table.ForeignKey(
                        name: "FK_Policier_HistGrade",
                        column: x => x.idPolicier,
                        principalTable: "Policier",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "Langue",
                columns: table => new
                {
                    id = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: false),
                    libelle = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false),
                    niveauEcriture = table.Column<int>(type: "int", nullable: false),
                    niveauLecture = table.Column<int>(type: "int", nullable: false),
                    idPolicier = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Langue_PK", x => x.id);
                    table.ForeignKey(
                        name: "FK_Policier_Langue",
                        column: x => x.idPolicier,
                        principalTable: "Policier",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "PersonnePrevenir",
                columns: table => new
                {
                    id = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: false),
                    nom = table.Column<string>(type: "varchar(30)", unicode: false, maxLength: 30, nullable: false),
                    postNom = table.Column<string>(type: "varchar(30)", unicode: false, maxLength: 30, nullable: true),
                    prenom = table.Column<string>(type: "varchar(30)", unicode: false, maxLength: 30, nullable: true),
                    numeroRue = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: false),
                    rue = table.Column<string>(type: "varchar(30)", unicode: false, maxLength: 30, nullable: false),
                    commune = table.Column<string>(type: "varchar(30)", unicode: false, maxLength: 30, nullable: false),
                    telephone = table.Column<string>(type: "varchar(30)", unicode: false, maxLength: 30, nullable: false),
                    idPolicier = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PersonnePrevenir_PK", x => x.id);
                    table.ForeignKey(
                        name: "FK_Policier_PersonnePrevenir",
                        column: x => x.idPolicier,
                        principalTable: "Policier",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "Sport",
                columns: table => new
                {
                    id = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: false),
                    libelle = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: false),
                    idPolicier = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Sport_PK", x => x.id);
                    table.ForeignKey(
                        name: "FK_Policier_Sport",
                        column: x => x.idPolicier,
                        principalTable: "Policier",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "RolePermission",
                columns: table => new
                {
                    idRole = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: false),
                    idPermission = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: false),
                    dateAttribution = table.Column<DateTime>(type: "datetime", nullable: false),
                    attribuePar = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("RolePermission_PK", x => new { x.idRole, x.idPermission });
                    table.ForeignKey(
                        name: "FK_RolePermission_Permission",
                        column: x => x.idPermission,
                        principalTable: "Permission",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RolePermission_Role",
                        column: x => x.idRole,
                        principalTable: "Role",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Utilisateur",
                columns: table => new
                {
                    id = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: false),
                    nomUtilisateur = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    motDePasse = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false),
                    nom = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    prenom = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    email = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    telephone = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: true),
                    estActif = table.Column<bool>(type: "bit", nullable: false),
                    dateCreation = table.Column<DateTime>(type: "datetime", nullable: false),
                    derniereConnexion = table.Column<DateTime>(type: "datetime", nullable: true),
                    motDePasseModifie = table.Column<bool>(type: "bit", nullable: false),
                    idEnqueteur = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: true),
                    idRole = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Utilisateur_PK", x => x.id);
                    table.ForeignKey(
                        name: "FK_Utilisateur_Enqueteur",
                        column: x => x.idEnqueteur,
                        principalTable: "Enqueteur",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_Utilisateur_Role",
                        column: x => x.idRole,
                        principalTable: "Role",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Audit",
                columns: table => new
                {
                    id = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: false),
                    idUtilisateur = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: false),
                    action = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    table = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    idEntite = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: true),
                    anciennesValeurs = table.Column<string>(type: "varchar(1000)", unicode: false, maxLength: 1000, nullable: true),
                    nouvellesValeurs = table.Column<string>(type: "varchar(1000)", unicode: false, maxLength: 1000, nullable: true),
                    dateAction = table.Column<DateTime>(type: "datetime", nullable: false),
                    adresseIP = table.Column<string>(type: "varchar(45)", unicode: false, maxLength: 45, nullable: true),
                    userAgent = table.Column<string>(type: "varchar(500)", unicode: false, maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Audit_PK", x => x.id);
                    table.ForeignKey(
                        name: "FK_Audit_Utilisateur",
                        column: x => x.idUtilisateur,
                        principalTable: "Utilisateur",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Audit_idUtilisateur",
                table: "Audit",
                column: "idUtilisateur");

            migrationBuilder.CreateIndex(
                name: "IX_Conjoint_idPolicier",
                table: "Conjoint",
                column: "idPolicier");

            migrationBuilder.CreateIndex(
                name: "IX_DistinctionHonorifique_idPolicier",
                table: "DistinctionHonorifique",
                column: "idPolicier");

            migrationBuilder.CreateIndex(
                name: "IX_Document_idPolicier",
                table: "Document",
                column: "idPolicier");

            migrationBuilder.CreateIndex(
                name: "IX_Empreinte_idPolicier",
                table: "Empreinte",
                column: "idPolicier");

            migrationBuilder.CreateIndex(
                name: "IX_Enfant_idPolicier",
                table: "Enfant",
                column: "idPolicier");

            migrationBuilder.CreateIndex(
                name: "IX_FonctionBefore1997_idCommissariat",
                table: "FonctionBefore1997",
                column: "idCommissariat");

            migrationBuilder.CreateIndex(
                name: "IX_Formation_idPolicier",
                table: "Formation",
                column: "idPolicier");

            migrationBuilder.CreateIndex(
                name: "IX_FRI_idCommissariat",
                table: "FRI",
                column: "idCommissariat");

            migrationBuilder.CreateIndex(
                name: "IX_FRI_idEnqueteur",
                table: "FRI",
                column: "idEnqueteur");

            migrationBuilder.CreateIndex(
                name: "IX_FRI_idPolicier",
                table: "FRI",
                column: "idPolicier");

            migrationBuilder.CreateIndex(
                name: "IX_HistAffectation_idPolicier",
                table: "HistAffectation",
                column: "idPolicier");

            migrationBuilder.CreateIndex(
                name: "IX_HistFonction_idPolicier",
                table: "HistFonction",
                column: "idPolicier");

            migrationBuilder.CreateIndex(
                name: "IX_HistGrade_idPolicier",
                table: "HistGrade",
                column: "idPolicier");

            migrationBuilder.CreateIndex(
                name: "IX_Langue_idPolicier",
                table: "Langue",
                column: "idPolicier");

            migrationBuilder.CreateIndex(
                name: "IX_Permission_Module_Action",
                table: "Permission",
                columns: new[] { "module", "action" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PersonnePrevenir_idPolicier",
                table: "PersonnePrevenir",
                column: "idPolicier");

            migrationBuilder.CreateIndex(
                name: "IX_Policier_idCommissariat",
                table: "Policier",
                column: "idCommissariat");

            migrationBuilder.CreateIndex(
                name: "IX_Role_Nom",
                table: "Role",
                column: "nom",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Role_PermissionId",
                table: "Role",
                column: "PermissionId");

            migrationBuilder.CreateIndex(
                name: "IX_RolePermission_idPermission",
                table: "RolePermission",
                column: "idPermission");

            migrationBuilder.CreateIndex(
                name: "IX_Sport_idPolicier",
                table: "Sport",
                column: "idPolicier");

            migrationBuilder.CreateIndex(
                name: "IX_Utilisateur_idEnqueteur",
                table: "Utilisateur",
                column: "idEnqueteur");

            migrationBuilder.CreateIndex(
                name: "IX_Utilisateur_idRole",
                table: "Utilisateur",
                column: "idRole");

            migrationBuilder.CreateIndex(
                name: "IX_Utilisateur_NomUtilisateur",
                table: "Utilisateur",
                column: "nomUtilisateur",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Audit");

            migrationBuilder.DropTable(
                name: "Conjoint");

            migrationBuilder.DropTable(
                name: "DistinctionHonorifique");

            migrationBuilder.DropTable(
                name: "Document");

            migrationBuilder.DropTable(
                name: "Empreinte");

            migrationBuilder.DropTable(
                name: "Enfant");

            migrationBuilder.DropTable(
                name: "FonctionBefore1997");

            migrationBuilder.DropTable(
                name: "Formation");

            migrationBuilder.DropTable(
                name: "FRI");

            migrationBuilder.DropTable(
                name: "HistAffectation");

            migrationBuilder.DropTable(
                name: "HistFonction");

            migrationBuilder.DropTable(
                name: "HistGrade");

            migrationBuilder.DropTable(
                name: "Langue");

            migrationBuilder.DropTable(
                name: "NUTP");

            migrationBuilder.DropTable(
                name: "PersonnePrevenir");

            migrationBuilder.DropTable(
                name: "RolePermission");

            migrationBuilder.DropTable(
                name: "Sport");

            migrationBuilder.DropTable(
                name: "Utilisateur");

            migrationBuilder.DropTable(
                name: "Policier");

            migrationBuilder.DropTable(
                name: "Enqueteur");

            migrationBuilder.DropTable(
                name: "Role");

            migrationBuilder.DropTable(
                name: "Commissariat");

            migrationBuilder.DropTable(
                name: "Permission");
        }
    }
}
