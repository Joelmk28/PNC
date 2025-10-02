using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PNC.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Créer la table Commissariat
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

            // Créer la table Role
            migrationBuilder.CreateTable(
                name: "Role",
                columns: table => new
                {
                    id = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: false),
                    nom = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    description = table.Column<string>(type: "varchar(200)", unicode: false, maxLength: 200, nullable: true),
                    estActif = table.Column<bool>(type: "bit", nullable: false),
                    dateCreation = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Role_PK", x => x.id);
                });

            // Créer la table Permission
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

            // Créer la table RolePermission
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
                        name: "FK_RolePermission_Role",
                        column: x => x.idRole,
                        principalTable: "Role",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RolePermission_Permission",
                        column: x => x.idPermission,
                        principalTable: "Permission",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            // Créer la table Utilisateur
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
                    idEnqueteur = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: true),
                    idRole = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Utilisateur_PK", x => x.id);
                    table.ForeignKey(
                        name: "FK_Utilisateur_Role",
                        column: x => x.idRole,
                        principalTable: "Role",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            // Créer la table Audit
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

            // Créer la table Policier
            migrationBuilder.CreateTable(
                name: "Policier",
                columns: table => new
                {
                    id = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: false),
                    numeroNutp = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false),
                    matricule = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false),
                    nom = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    postNom = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    prenom = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    sexe = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: false),
                    dateNaissance = table.Column<DateTime>(type: "datetime", nullable: false),
                    jour = table.Column<string>(type: "varchar(2)", unicode: false, maxLength: 2, nullable: false),
                    mois = table.Column<string>(type: "varchar(2)", unicode: false, maxLength: 2, nullable: false),
                    annee = table.Column<int>(type: "int", nullable: false),
                    paysNaissance = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    villeNaissance = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    villageNaissance = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    nationalite = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    etatCivil = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false),
                    adresse = table.Column<string>(type: "varchar(200)", unicode: false, maxLength: 200, nullable: false),
                    telephone = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: true),
                    email = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    dateRecrutement = table.Column<DateTime>(type: "datetime", nullable: false),
                    grade = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    uniteAffectation = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false),
                    observation = table.Column<string>(type: "varchar(500)", unicode: false, maxLength: 500, nullable: false),
                    idCommissariat = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: true)
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

            // Créer la table Enqueteur
            migrationBuilder.CreateTable(
                name: "Enqueteur",
                columns: table => new
                {
                    id = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: false),
                    numeroMatricule = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false),
                    nom = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    postNom = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    prenom = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    sexe = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: false),
                    dateNaissance = table.Column<DateTime>(type: "datetime", nullable: false),
                    nationalite = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    adresse = table.Column<string>(type: "varchar(200)", unicode: false, maxLength: 200, nullable: false),
                    telephone = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: true),
                    email = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    dateRecrutement = table.Column<DateTime>(type: "datetime", nullable: false),
                    grade = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    specialite = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    observation = table.Column<string>(type: "varchar(500)", unicode: false, maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Enqueteur_PK", x => x.id);
                });

            // Créer la table Fri
            migrationBuilder.CreateTable(
                name: "Fri",
                columns: table => new
                {
                    id = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: false),
                    dateRemplissage = table.Column<DateTime>(type: "datetime", nullable: false),
                    jour = table.Column<string>(type: "varchar(2)", unicode: false, maxLength: 2, nullable: false),
                    mois = table.Column<string>(type: "varchar(2)", unicode: false, maxLength: 2, nullable: false),
                    annee = table.Column<int>(type: "int", nullable: false),
                    numero = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false),
                    idEnqueteur = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: false),
                    idPolicier = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: false),
                    idCommissariat = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: false),
                    observation = table.Column<string>(type: "varchar(500)", unicode: false, maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Fri_PK", x => x.id);
                    table.ForeignKey(
                        name: "FK_Fri_Enqueteur",
                        column: x => x.idEnqueteur,
                        principalTable: "Enqueteur",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Fri_Policier",
                        column: x => x.idPolicier,
                        principalTable: "Policier",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Fri_Commissariat",
                        column: x => x.idCommissariat,
                        principalTable: "Commissariat",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            // Créer les index uniques
            migrationBuilder.CreateIndex(
                name: "IX_Utilisateur_NomUtilisateur",
                table: "Utilisateur",
                column: "nomUtilisateur",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Role_Nom",
                table: "Role",
                column: "nom",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Permission_Module_Action",
                table: "Permission",
                columns: new[] { "module", "action" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Policier_NumeroNutp",
                table: "Policier",
                column: "numeroNutp",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Policier_Matricule",
                table: "Policier",
                column: "matricule",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Enqueteur_NumeroMatricule",
                table: "Enqueteur",
                column: "numeroMatricule",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Supprimer les tables dans l'ordre inverse
            migrationBuilder.DropTable(name: "Fri");
            migrationBuilder.DropTable(name: "Policier");
            migrationBuilder.DropTable(name: "Enqueteur");
            migrationBuilder.DropTable(name: "Audit");
            migrationBuilder.DropTable(name: "Utilisateur");
            migrationBuilder.DropTable(name: "RolePermission");
            migrationBuilder.DropTable(name: "Permission");
            migrationBuilder.DropTable(name: "Role");
            migrationBuilder.DropTable(name: "Commissariat");
        }
    }
}
