using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace StudyQuest.API.Migrations
{
    /// <inheritdoc />
    public partial class WASSCESyllabusUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Clean up user data with RESTRICT foreign keys to old Topics/Subjects
            // before the seed data replacement deletes them.
            migrationBuilder.Sql("""
                DELETE FROM "QuizQuestions" WHERE "QuizId" IN (SELECT "Id" FROM "Quizzes");
                DELETE FROM "Quizzes" WHERE "TopicId" IN (SELECT "Id" FROM "Topics");
                DELETE FROM "StudyPlanItems" WHERE "TopicId" IN (SELECT "Id" FROM "Topics");
                DELETE FROM "StudyPlans" WHERE "SubjectId" IN (SELECT "Id" FROM "Subjects");
                DELETE FROM "TimetableEntries" WHERE "SubjectId" IN (SELECT "Id" FROM "Subjects");
                DELETE FROM "StudySessions" WHERE "SubjectId" IN (SELECT "Id" FROM "Subjects");
                DELETE FROM "Enrollments" WHERE "SubjectId" IN (SELECT "Id" FROM "Subjects");
                DELETE FROM "StudentProgress" WHERE "SubjectId" IN (SELECT "Id" FROM "Subjects");
                """);

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("01b83c0a-da1b-d3ee-63c6-a7223a562a70"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("01c89125-6054-4ff5-732e-e588e2723bd7"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("0418668d-5ea4-5b7c-9fa6-1f46a2b4ae82"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("050fd061-3d93-5f39-8a4a-c324dfe3ae55"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("0542c6bb-6c8c-3d9a-e9fc-af56459051ed"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("0618c3f0-291c-6ede-9cd8-2cb9f28e7d8c"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("0651b6ce-8c9c-a631-9a5c-832a62d3f4ce"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("0780c4e7-f729-4e93-dcd4-e97c01c6abe6"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("09e08e66-3440-ad62-4e19-33609a335577"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("0a968297-5849-7e33-62a7-42614e560e53"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("0b886e2a-37b2-806a-17ff-af7cbfd75b1f"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("0b9021c9-d560-a851-92cc-d490c449b774"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("0d051784-8b5c-c90f-7db0-eea0b82325e6"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("0e351883-931d-6518-94a2-8bfa033b954c"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("0f004646-1571-b5d5-f8fe-ae1d2a422952"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("12c332eb-1189-88ff-d2cf-1491009c5e64"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("14ce031f-6c0a-6af2-279f-4b6e66c27169"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("197831c0-c31f-305f-5fca-6809f0d96f7f"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("214ed8ad-1898-327b-07df-f8ae143bbc41"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("231ced44-3f9d-42fa-aa02-7ace491139be"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("26436f95-e13e-e1db-1a17-740a78be7ce5"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("2887d4df-b719-9850-3b71-d91448d76cd0"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("288d8ea0-c1fb-b3e6-7972-dd554017287a"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("297b9215-1ec3-f632-2c1a-7e790fcdfb57"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("2a981624-ea72-0c90-5f56-cebe5168cb33"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("2d679eb7-979c-625c-f1d5-2a46d2eaba03"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("2d9f2e39-601b-8d59-3057-19d92171c6cf"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("2e1204bc-b7c0-831f-bf12-d8c8dbd271e5"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("2ec7ecd2-b943-bd77-8efc-b01725d6133a"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("2f03fddb-8947-45ea-224c-cd9e9cc8514e"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("2f5c42c7-5b66-043f-a338-ff1489cb84b4"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("2ff2ff99-92d5-5e48-22cd-999d6bcdbe81"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("32543d9f-dba6-d777-98b6-fb1248a26f82"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("3284ed1c-bfd2-e1b4-39ef-b396f35857ad"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("333d6e57-1224-bdd4-f8ed-972af8e887dd"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("34986eb4-6e8f-4ae3-c52b-8c4dd81799e3"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("34ac8896-f538-98f6-045b-d6eac23845fc"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("35c791a5-93f6-d9c6-fba4-2813a98e1c27"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("37cc5473-c3e9-fdb3-8066-e41147b900d3"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("394a31f0-6f35-1637-f69a-8cb7b9a306b5"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("3b2fc17f-e7dc-f10e-6aef-d74f8f70428e"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("3cd5f9e7-e4d7-93ad-6b57-8960015fda54"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("3df6b272-c703-ea94-590d-39cf4c886223"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("3e7dd8ba-27be-47e9-7819-37c77520a90d"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("418e8e0c-5c2c-7bb2-6827-b00bbf80bb39"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("41c2f514-4908-cdb9-8cf8-ab12c6dd82bb"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("421ebeb7-f096-909d-bbed-88241ea8f5fd"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("45a27f2a-c0fe-2439-003e-bbb6b6a6fcf5"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("46016da9-d7e5-79c3-42ed-7f9e781ad409"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("469a9107-71ed-3a47-6f04-4c66303ac5e3"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("46a1de31-acbc-fa47-f1d9-a4c405ad4c06"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("48751395-d242-5f84-48c6-dd188eb243e3"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("49274ac5-49e2-98a0-0f81-21ed00496707"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("4a7b80cb-3322-d226-5d04-7bef5cd1427d"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("4c9b3c0e-ea03-6dc1-edca-b11ae4d2d37d"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("504dfee0-22b2-3f84-66f8-0866506b2167"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("50de216c-0ddc-ae74-f8ad-5444e814b235"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("51359da3-e266-2b11-205d-1d3e634654c7"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("51ef4d2e-6f88-4f2f-1fd2-185a6afe78b9"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("5262754b-d02f-be07-3cc5-81f92daf5e45"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("52a5fcb0-3134-3b41-f1ae-d181ba532f09"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("532239c5-336a-48f8-cf92-a9651fc66796"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("53da0239-03de-447c-e7d5-0ac404ee6ce8"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("54110ec1-7c50-6e4c-b811-c4dd1df5485a"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("54cebaf0-c5b2-446b-017d-a3b10deb3679"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("58648486-b170-4c7d-f262-5ae496ea71de"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("58ecad01-ddfb-38eb-e236-388b371ea195"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("59d26f10-1011-730a-bb2a-da01ad019205"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("5a6e7f96-d196-5905-45a2-408144c11403"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("5a75752a-5a4c-ab5e-cc8f-b5ec4d3cea1b"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("5a765634-6417-4f5e-1a33-b45063aa04f3"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("5ea08431-8e62-9a42-e6d8-2be18c18db18"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("5fbc9b9f-3366-4659-dd89-9cf00a28b73a"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("60f06aab-ede4-cd69-1197-dfe54ecd27ec"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("61542b76-fc35-064b-7dfb-ed661ab26961"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("615c5357-e20c-8d6a-db74-dc5e43b353ad"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("6288f6fc-0672-c122-b2b9-df5d967376bc"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("639bf567-8933-910e-f99e-3033e568d205"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("63ad600b-8bca-1b26-eade-64d0a219a279"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("6c6e3005-6ee3-bbe6-d150-eb76b67a30ca"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("6d43e975-bfc9-fca1-c243-de13a1063642"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("6dfcc605-241e-9b04-b6bb-35aed8cc13cc"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("6ef9764f-16b5-a8de-e373-6f9f7bbf35e4"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("6f52470d-b0fa-604c-ad84-15bae8e1a8b2"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("70d9d2a8-636e-3488-4edb-cc64cf300f77"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("70f888ef-b3d2-f258-7cbe-3feab9adba0d"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("716f6983-9029-6713-64bd-776749d4500c"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("718dcda4-4d62-267f-8ac8-f05a1c9d2726"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("72272db0-2456-e9d7-4e47-7eab7c5a961a"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("72499899-2012-50f0-baca-b8267f238437"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("7431c6fe-376b-4b07-b480-d36203e1e9df"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("7482d60f-e6a9-4de5-e0c0-9a664877752f"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("74ebf86b-b31b-5327-dc77-d43bc9f9ee18"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("7571f553-46da-20c5-75db-9943be088f12"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("76040032-7e9e-5cc1-a3a8-41225f37ed3b"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("76f4b310-7843-c068-1452-a3c0c8352ade"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("771514d7-f986-ab4f-9253-413fb16300f1"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("781aede2-451d-e8a1-0583-bdb2657a62b4"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("792e1ddb-b494-4edd-dbfe-0e51a35071d5"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("7a6038e9-d2e5-cbc0-fe36-4d39c79939a5"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("7d8d7a19-7d81-d3c9-47c7-ac7cebadf41a"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("7dfd8bfa-4134-5dc1-7041-04661e427d6b"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("7f275686-7a4e-77cf-fb60-f5e7020f1a06"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("81004de3-ba78-1bbd-8090-be0cda89866c"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("81788eec-1a1c-14e9-548c-44f7e31e74c4"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("82b597dd-e1dc-3398-27ac-c4a9bbe80b44"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("832be98d-ff03-a21e-b1ec-34e30faca608"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("83321fec-3af1-8082-dbbf-9aa7cf4acebb"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("837f11aa-9d94-bd5c-6db6-fb2e8ec2c506"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("839e9bb7-cb14-f554-d1c5-cd0dde7f5784"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("854a8a9e-7b89-c9fb-4811-4c5d9d19023b"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("867d475d-0238-17ca-510c-320f7f616248"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("86ce8d84-408d-9d4c-adf2-8d16d69423e4"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("88d3aed4-7aa6-aeaf-be74-3f3c44f8fe91"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("8a4cb3cf-d79d-8b4e-e9cc-bd45a903fb90"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("8b2a8163-39ae-a6f0-d25d-5c05a22bd365"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("8b929c5a-3418-4a3f-982c-47939f2de55b"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("8d715e79-be6e-5e13-10c9-2185d6fa5076"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("8d739583-8bb6-1144-d638-3130c8670a85"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("8df031a3-dc4a-74cb-8cea-84482e97e0cc"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("8fe6402d-ab28-170b-b190-59519649e6a0"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("900b0581-a884-0700-5c2f-1ee846b2d9a6"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("91a31b80-6aaa-df03-65d7-616e68adcbb9"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("91d90182-e3de-3d69-34d2-fd07abb6710a"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("92141092-e9bc-e505-2aca-35d46b603441"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("921d990b-d5de-edf1-c136-335b1b60a23d"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("92579598-4144-1b60-efe1-d108270bee32"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("92bfd75c-ad93-0d18-c3bf-1dbfc0062da0"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("92c8e20b-a633-3a98-a238-b618bc716f8b"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("9353cb3e-0beb-f23b-c2f4-214538abf8be"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("93891937-80ee-7db9-5ded-c76f297baf24"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("93dd70db-f471-0287-6b4a-088b0afdc4e9"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("94a0ed13-a568-1e7f-94d6-a362cbb000a2"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("958ef24a-1848-11fc-7260-6ca5fb0e08b5"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("972dc873-9109-24de-9ff9-fd076b1783d4"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("982a1d25-91f5-061f-eec2-903b4fd07a5b"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("988c83c1-55da-d3e2-12aa-de74d132c448"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("98dd9392-0a4f-01ca-5bf8-3070c618379b"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("9a7e23fa-7cd8-7df9-3ebf-04d780ad7bb8"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("9ac07bb9-773c-3e1a-3dbf-fd306f613c94"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("9b431cb9-474e-8611-1199-e207dd49740a"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("9c623dd8-3a7c-8aad-0601-f0a84074c3e9"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("9d5c3fe7-8b62-b3c7-dde8-82fb9da0350b"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("9d6e7e68-fc31-da6c-7467-2adbc7ee1638"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("9d81c63d-ced0-fb8a-0dec-22f293b69ae4"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("9df9e604-aa11-b7f5-eefa-56adc54d807c"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("9f3ff1df-ad72-2908-3f6b-2bc02348a496"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("9fa00bb7-65bf-0e73-7a34-d9f776af9cdb"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("a10948f2-bb20-75fa-7829-1d374f32ff72"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("a1ec96b8-4bf8-6b02-4a31-694e5157a900"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("a229c887-61eb-61c3-82a1-8fc37af2a58e"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("a27e3586-6d0e-ba1e-0584-df1d3557d392"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("a3de0b84-1ab3-2f97-5ddb-2a72dffd9fb2"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("a467b2c6-0c89-9bf5-2d65-016d07ea87e3"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("a4c9b011-4ccd-76c9-ddbf-7290777f735f"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("a8039e66-9ee4-9b91-0790-b563c3701ded"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("a82eb8a4-4575-69b4-27ba-42261f0dd677"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("a96f56d1-fcda-bdb6-e26a-add35e069baf"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("aad284ca-133a-8645-69d8-b8b3d90e1cb9"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("ab210d31-92e1-b37c-ef4a-aaa6e59542d8"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("abaa3fa4-c13c-5e90-6fda-f781cd3c883a"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("acd2944d-8102-2c04-b518-e0520dc91d6b"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("adaef9b7-1a85-d8bc-c690-83288f70c2d6"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("ae58b202-01db-0196-287e-57f047e49539"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("af4be5ec-c9c7-cb98-28e8-555888526a6a"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("af6df267-d7f7-40aa-bac4-97a62cdb4812"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("b03e4dd7-c5ee-6122-294d-5cf2c6543156"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("b0b02cb0-f0eb-a064-ee36-75ea48b10fe5"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("b189674c-4e35-7dae-e043-007edf702650"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("b25ad283-d4a3-a8e3-e2a3-3b2680f3be1e"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("b44f7958-dee5-6736-648c-6e871f906d3f"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("b4a4b05f-7229-0b1e-5112-5fbe330f87c5"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("b578853a-5577-6231-4ead-4700bcdbe6fd"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("bb7b2c32-22c4-7ee8-84bb-1d80f3312ea3"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("bbf12c25-de6f-1e64-1c1a-4cd09ccdc5c7"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("bf10904f-67bc-7ab4-8118-404a265badc2"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("bff01c9b-9947-3576-9732-8ebe11eabebd"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("c094121f-5733-f5bf-3495-25546af2bc95"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("c0a479ce-2f71-6717-c032-3daaf8641b3a"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("c0e20d79-e15b-7eac-ce2c-2c225464fbc7"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("c15d5a7b-0450-1976-7333-fb5cb0d0a05c"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("c2c2dc37-85b5-b179-a185-3a817633d54a"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("c306c6c6-0139-cef5-3df7-8ccaaf61903c"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("c482fee0-eaa6-b70d-b94e-19495a18b0c8"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("c593755e-43a2-bee1-efd8-ab112688b7d3"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("c5dfcf02-6cc0-0435-efd5-4a5936dbaa16"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("c5e93b39-4278-eae7-3ac5-22b60c087068"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("c637c9c3-e71e-ef19-b93c-038e1060c339"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("c677ac5a-d29a-8fb7-a71d-3986e156e8ab"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("c6cf9add-0ec4-5b53-e4f8-65a523a9749b"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("c8259e20-c95c-2239-fd1f-c9d15bfababc"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("c8ad3461-3649-93da-ed28-595297e891c3"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("c8b0d99d-86a7-208f-43a9-f94003608d3b"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("c8cfe728-fe0a-0883-ad14-0057aef8697e"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("c8e3da43-f33b-c2f1-d9db-f6553cf6ce64"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("c908eff7-1df5-09a9-f5ae-3617899b6f31"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("c99c9b78-3b6b-5e2f-d6b3-57cb0918ec66"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("cabfdf95-7580-c4f0-9d96-e354a9d1d823"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("cc0faa1b-b9b5-49b5-1cc2-9514bdab0a64"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("cd33959b-8ba9-cb96-3898-34c93816e98f"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("cdd08660-6404-dd4b-4da7-f58d68ae3897"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("d02c0f9a-e3f0-499e-744f-1555b82d3581"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("d03460f3-f948-4426-bdbf-8c091519d492"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("d4770125-312b-dd21-7945-d24510e977f5"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("d577391d-8052-ee84-1167-f7a74473a137"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("d57cb817-d7f3-efd6-aae8-ea320ccba361"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("d8eb0352-ad2a-b199-aa10-a4db9eb9e308"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("db34d196-bee5-f55a-ac42-12b3644bca05"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("ddeb7bb0-2796-b4f5-151b-55ad39bd4715"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("df30a84d-9cb6-128b-745f-2a71a232f4ec"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("df945f07-f6fd-d786-9e3a-ca853df8eddf"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("df9b8207-2991-efd2-9864-5cd2fe8d486f"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("e035f078-80d9-779b-beca-f25499bf63c4"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("e0c6257b-a5de-6c40-b726-1aceac8e2f9a"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("e4e71981-b951-05a5-37af-a4c1ab890849"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("e5ab0208-183d-a059-c5ff-3c862c31674a"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("e5ea993b-9f92-26fd-9449-25fb109b2340"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("e624014c-6042-8ce8-ebc8-21db8dffceee"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("e88283a1-61e0-ee45-46c6-693bdca5ee44"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("e951ea1c-8c9d-cc2b-6b45-c0916476bbd0"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("ea9ecd93-dd47-0dfc-805c-d22f1422484f"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("eab93840-1960-cecc-c7c8-928cd373fc22"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("ecb99fdd-8c39-dba9-8237-2e95125945f8"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("ecda0c05-a8b9-5244-b3dd-743ebfb367b0"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("ed750b69-66a5-81ae-b3ad-912c2e5e7ba3"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("f1499460-3482-edd8-102f-374954f392ce"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("f2171eb6-9c3d-271d-9302-bd5d1bf36cdb"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("f36dfb07-4b90-fba9-f074-15dc22a07029"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("f38c2c9d-e19b-8359-480c-67c3669b662f"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("f39fa9a8-b9fe-a19d-c71a-b9350eecf920"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("f42371cf-aaf2-cfa0-994c-cc134d4fa9ad"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("f57ba5bd-1825-3bfd-616a-861b4eeae95d"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("f5ab6ef6-952a-47fa-81a2-1aa7aeb10694"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("f715ea05-b1ce-5c66-95f0-5a7ad3e6fed6"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("f7e8d9d9-e003-667e-a79b-867704c18363"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("f9f3697d-74dc-5cac-eaf3-9dc2d7c4e5f9"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("fe526bf2-961d-f568-89bf-28c52fc115a2"));

            migrationBuilder.DeleteData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("082d128a-6ee4-2c5d-0efc-f9b0b9af4c23"));

            migrationBuilder.DeleteData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("532a8048-546e-3d68-aa5a-14ae71eb6f9e"));

            migrationBuilder.DeleteData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("82786b28-5a6d-b0b6-194d-ab131764064d"));

            migrationBuilder.DeleteData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("89bdf99e-82a8-29b3-57d7-410b42d8dcf8"));

            migrationBuilder.DeleteData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("99b68af6-34fc-713c-884f-c018c6b17d72"));

            migrationBuilder.DeleteData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("fdca9ad9-bc36-79d0-421e-38c118a6c3cf"));

            migrationBuilder.UpdateData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("06216eaa-a8e4-6710-e535-da8f3f9af209"),
                column: "Description",
                value: "Biology — WASSCE (Grade 12)");

            migrationBuilder.UpdateData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("26a00ceb-e8ed-7c0b-b245-a60b6107d495"),
                column: "Description",
                value: "Chemistry — WASSCE (Grade 12)");

            migrationBuilder.UpdateData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("45c2e4c9-1dad-eb06-8108-5a7b253db61d"),
                column: "Description",
                value: "Chemistry — WASSCE (Grade 10)");

            migrationBuilder.UpdateData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("5295e6aa-8238-0794-a9ec-29deb0bef2aa"),
                column: "Description",
                value: "Mathematics — WASSCE (Grade 11)");

            migrationBuilder.UpdateData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("629825b8-c2d2-7aee-d163-0be88495e272"),
                column: "Description",
                value: "Geography — WASSCE (Grade 11)");

            migrationBuilder.UpdateData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("65ac38a6-5a9f-22e8-0de0-2181565e0d5e"),
                column: "Description",
                value: "Physics — WASSCE (Grade 12)");

            migrationBuilder.UpdateData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("69e6f790-ae65-2e87-f351-982b99d92bfe"),
                column: "Description",
                value: "Agriculture — WASSCE (Grade 12)");

            migrationBuilder.UpdateData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("6aaf47ef-e5a0-d4af-70db-f2011158ef8e"),
                column: "Description",
                value: "Biology — WASSCE (Grade 10)");

            migrationBuilder.UpdateData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("76324b9e-5fc2-7160-7522-932866c0a77a"),
                column: "Description",
                value: "Agriculture — WASSCE (Grade 11)");

            migrationBuilder.UpdateData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("7a475323-7df5-f613-6449-fcfad4461d7c"),
                column: "Description",
                value: "Geography — WASSCE (Grade 12)");

            migrationBuilder.UpdateData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("8c039749-e2dc-12fc-0757-80e63a219052"),
                column: "Description",
                value: "Physics — WASSCE (Grade 11)");

            migrationBuilder.UpdateData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("b862ea9a-d88a-6230-aeea-858a90d5a959"),
                column: "Description",
                value: "Biology — WASSCE (Grade 11)");

            migrationBuilder.UpdateData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("caea75ab-707a-872b-c92f-5500afab7afb"),
                column: "Description",
                value: "Physics — WASSCE (Grade 10)");

            migrationBuilder.UpdateData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("e37e03f0-c9ec-1714-3ff7-408956f8a8c0"),
                column: "Description",
                value: "Agriculture — WASSCE (Grade 10)");

            migrationBuilder.UpdateData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("eb63371f-c483-4b34-cf09-1f1f6825f8ba"),
                column: "Description",
                value: "Mathematics — WASSCE (Grade 12)");

            migrationBuilder.UpdateData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("f71b0725-2ab4-36b8-c08a-d709a40e16f4"),
                column: "Description",
                value: "Chemistry — WASSCE (Grade 11)");

            migrationBuilder.UpdateData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("f8380400-ac39-9946-15e0-894ba7e520e6"),
                column: "Description",
                value: "Geography — WASSCE (Grade 10)");

            migrationBuilder.UpdateData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("f9d10be1-9d0d-14ec-37e4-3ab4dfc611f8"),
                column: "Description",
                value: "Mathematics — WASSCE (Grade 10)");

            migrationBuilder.InsertData(
                table: "Subjects",
                columns: new[] { "Id", "Color", "Description", "Grade", "Name" },
                values: new object[,]
                {
                    { new Guid("029f27a3-acc3-f8ab-b362-4a0d9c1db6f6"), "#eab308", "Islamic Religious Studies — WASSCE (Grade 11)", 11, "Islamic Religious Studies" },
                    { new Guid("04fbf1c3-6b48-d6c9-e60b-8af777b7fa50"), "#10b981", "Economics — WASSCE (Grade 11)", 11, "Economics" },
                    { new Guid("0a0e509f-a99c-3193-c277-928d54224cc2"), "#eab308", "Islamic Religious Studies — WASSCE (Grade 12)", 12, "Islamic Religious Studies" },
                    { new Guid("0ba6c666-e140-2220-8ec4-4f16bb2cd61b"), "#f97316", "Commerce — WASSCE (Grade 10)", 10, "Commerce" },
                    { new Guid("0d331ad8-0343-b862-4378-b5f017817a63"), "#10b981", "Economics — WASSCE (Grade 12)", 12, "Economics" },
                    { new Guid("18c23abd-93c6-0e94-492c-dfd6d0a2ce28"), "#8b5cf6", "History — WASSCE (Grade 10)", 10, "History" },
                    { new Guid("30f1307e-bbbc-d420-61c7-124673a9fd1c"), "#0ea5e9", "Computer Studies — WASSCE (Grade 12)", 12, "Computer Studies" },
                    { new Guid("30f59780-4140-3f0a-cded-8b13fd7052cb"), "#f43f5e", "Christian Religious Studies — WASSCE (Grade 12)", 12, "Christian Religious Studies" },
                    { new Guid("35b0b395-4f39-535e-d197-cb82f839fbbc"), "#f43f5e", "Christian Religious Studies — WASSCE (Grade 10)", 10, "Christian Religious Studies" },
                    { new Guid("3c22945c-cc6c-6199-f614-746cb565217c"), "#65a30d", "Horticulture — WASSCE (Grade 10)", 10, "Horticulture" },
                    { new Guid("43daec58-ceb4-fc4f-f1d4-086bd79d72a8"), "#0ea5e9", "Computer Studies — WASSCE (Grade 10)", 10, "Computer Studies" },
                    { new Guid("49841f61-c212-8eec-2810-f1bb824e37d2"), "#e879f9", "Floriculture — WASSCE (Grade 11)", 11, "Floriculture" },
                    { new Guid("4b421fd5-92e6-ef02-edd7-7804412bedd0"), "#e879f9", "Floriculture — WASSCE (Grade 10)", 10, "Floriculture" },
                    { new Guid("4d512f11-4373-8e2f-543c-0842a91f4cf2"), "#14b8a6", "Financial Accounting — WASSCE (Grade 12)", 12, "Financial Accounting" },
                    { new Guid("598041c3-b226-5552-6f56-7b081f4d8279"), "#eab308", "Islamic Religious Studies — WASSCE (Grade 10)", 10, "Islamic Religious Studies" },
                    { new Guid("661ec45c-8411-3167-8af8-544152cebce6"), "#6366f1", "Government — WASSCE (Grade 12)", 12, "Government" },
                    { new Guid("6e0a7776-4921-0851-f05d-2a9cfe334aad"), "#f97316", "Commerce — WASSCE (Grade 12)", 12, "Commerce" },
                    { new Guid("70a66f3c-d5f9-1b35-bc9c-119ef3d85f3e"), "#0891b2", "Fisheries — WASSCE (Grade 10)", 10, "Fisheries" },
                    { new Guid("773db100-257e-8d22-3695-0e647cbb7fd4"), "#f97316", "Commerce — WASSCE (Grade 11)", 11, "Commerce" },
                    { new Guid("77d36d32-edfb-63a5-b573-c9b18f58bb17"), "#0891b2", "Fisheries — WASSCE (Grade 11)", 11, "Fisheries" },
                    { new Guid("8c240df7-cbb8-b63b-c2fa-15ef2ff299d6"), "#8b5cf6", "History — WASSCE (Grade 12)", 12, "History" },
                    { new Guid("94f51606-1b58-9322-ecb2-cc8be90adce0"), "#10b981", "Economics — WASSCE (Grade 10)", 10, "Economics" },
                    { new Guid("97ec5abf-b57f-0843-fe64-8f31af424c93"), "#f43f5e", "Christian Religious Studies — WASSCE (Grade 11)", 11, "Christian Religious Studies" },
                    { new Guid("9e7459ec-d351-be24-197d-c29757abab72"), "#3b82f6", "English Language — WASSCE (Grade 12)", 12, "English Language" },
                    { new Guid("a115558a-224b-bd96-8793-629375a2869e"), "#d946ef", "Further Mathematics — WASSCE (Grade 11)", 11, "Further Mathematics" },
                    { new Guid("a33a76d9-ae1b-fb53-fef6-83cfc12992bb"), "#8b5cf6", "History — WASSCE (Grade 11)", 11, "History" },
                    { new Guid("a4eaeeb3-b7a3-9abd-1c53-5309c8d473c0"), "#d946ef", "Further Mathematics — WASSCE (Grade 10)", 10, "Further Mathematics" },
                    { new Guid("aea3fc29-a3f7-76c0-8be7-f73335d78889"), "#0891b2", "Fisheries — WASSCE (Grade 12)", 12, "Fisheries" },
                    { new Guid("c0e3b77a-b7e6-f84c-1414-a9b4986364c7"), "#6366f1", "Government — WASSCE (Grade 10)", 10, "Government" },
                    { new Guid("c0eae66d-d3fd-8079-f377-be72f93a2d06"), "#d946ef", "Further Mathematics — WASSCE (Grade 12)", 12, "Further Mathematics" },
                    { new Guid("c33eef6f-8e20-9183-f5a2-4e5187de42d7"), "#14b8a6", "Financial Accounting — WASSCE (Grade 11)", 11, "Financial Accounting" },
                    { new Guid("c5ad46a4-1f10-7aab-6b9d-9d32bca37e63"), "#6366f1", "Government — WASSCE (Grade 11)", 11, "Government" },
                    { new Guid("cd368ced-9b4b-53c2-f818-261143f97c4b"), "#14b8a6", "Financial Accounting — WASSCE (Grade 10)", 10, "Financial Accounting" },
                    { new Guid("def2214a-a301-249e-c656-0914b4ecb5a5"), "#3b82f6", "English Language — WASSCE (Grade 10)", 10, "English Language" },
                    { new Guid("e868e7f9-be26-b7fe-6bff-f3ca7fd9b926"), "#65a30d", "Horticulture — WASSCE (Grade 12)", 12, "Horticulture" },
                    { new Guid("efa84916-91b0-0dea-73a8-860fc9a80941"), "#65a30d", "Horticulture — WASSCE (Grade 11)", 11, "Horticulture" },
                    { new Guid("f081fc93-cdb6-c836-9743-149f52228aab"), "#3b82f6", "English Language — WASSCE (Grade 11)", 11, "English Language" },
                    { new Guid("f1ddfb49-3ee7-d0d0-3bdb-ba71e5d13aec"), "#e879f9", "Floriculture — WASSCE (Grade 12)", 12, "Floriculture" },
                    { new Guid("fffdd2fc-d6d3-7308-9d25-267ff832fd7f"), "#0ea5e9", "Computer Studies — WASSCE (Grade 11)", 11, "Computer Studies" }
                });

            migrationBuilder.UpdateData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("c86aaefa-61f2-1f82-6ecd-9243160261b4"),
                column: "Order",
                value: 10);

            migrationBuilder.InsertData(
                table: "Topics",
                columns: new[] { "Id", "Description", "Name", "Order", "SubjectId" },
                values: new object[,]
                {
                    { new Guid("0042fff6-7eea-606b-73fa-3c4353582545"), "Growth and Development — Biology Grade 11", "Growth and Development", 5, new Guid("b862ea9a-d88a-6230-aeea-858a90d5a959") },
                    { new Guid("00988525-1016-8011-b62c-8d3d403a9de1"), "Skeletal and Muscular Systems — Biology Grade 11", "Skeletal and Muscular Systems", 9, new Guid("b862ea9a-d88a-6230-aeea-858a90d5a959") },
                    { new Guid("03c7938c-2c9a-ba3a-d4a1-17789fa4a4a7"), "Electromagnetic Spectrum — Physics Grade 12", "Electromagnetic Spectrum", 9, new Guid("65ac38a6-5a9f-22e8-0de0-2181565e0d5e") },
                    { new Guid("049f518d-2c42-97a2-07ec-c8fef7249909"), "Industrial Chemistry (Extraction of Metals) — Chemistry Grade 12", "Industrial Chemistry (Extraction of Metals)", 4, new Guid("26a00ceb-e8ed-7c0b-b245-a60b6107d495") },
                    { new Guid("0574f44a-0305-dd34-001b-b769cc27bfe4"), "Transformation Geometry — Mathematics Grade 12", "Transformation Geometry", 4, new Guid("eb63371f-c483-4b34-cf09-1f1f6825f8ba") },
                    { new Guid("073707a7-6f14-68f1-db49-6cb749dfa93b"), "WASSCE Past Questions Practice — Physics Grade 12", "WASSCE Past Questions Practice", 11, new Guid("65ac38a6-5a9f-22e8-0de0-2181565e0d5e") },
                    { new Guid("0854ecc8-a190-5282-e4dc-26febd613262"), "Pest and Disease Control in Crops — Agriculture Grade 10", "Pest and Disease Control in Crops", 9, new Guid("e37e03f0-c9ec-1714-3ff7-408956f8a8c0") },
                    { new Guid("086fe156-0cbc-1ec3-922b-5649cedca154"), "Qualitative Analysis — Chemistry Grade 11", "Qualitative Analysis", 10, new Guid("f71b0725-2ab4-36b8-c08a-d709a40e16f4") },
                    { new Guid("0ad004f1-2308-0ad0-b1b3-9ab0de48fa00"), "Simultaneous Equations — Mathematics Grade 11", "Simultaneous Equations", 3, new Guid("5295e6aa-8238-0794-a9ec-29deb0bef2aa") },
                    { new Guid("10fbfb84-01eb-a793-5ff0-b84fc359f916"), "Number and Numeration — Mathematics Grade 10", "Number and Numeration", 1, new Guid("f9d10be1-9d0d-14ec-37e4-3ab4dfc611f8") },
                    { new Guid("13e0ac9a-4f6d-74d5-baf6-68c752bbe1b3"), "Non-Metals and Their Compounds — Chemistry Grade 11", "Non-Metals and Their Compounds", 9, new Guid("f71b0725-2ab4-36b8-c08a-d709a40e16f4") },
                    { new Guid("16ef2301-77be-2843-21c8-448971c86c22"), "Mensuration (Perimeter, Area, Volume) — Mathematics Grade 10", "Mensuration (Perimeter, Area, Volume)", 7, new Guid("f9d10be1-9d0d-14ec-37e4-3ab4dfc611f8") },
                    { new Guid("176bb61b-e3b0-5d6a-f035-b6bb4a6cc315"), "WASSCE Past Questions Practice — Mathematics Grade 12", "WASSCE Past Questions Practice", 11, new Guid("eb63371f-c483-4b34-cf09-1f1f6825f8ba") },
                    { new Guid("1af1dc5d-81d4-24c1-146b-45f8ccf45020"), "Linear Momentum and Collisions — Physics Grade 11", "Linear Momentum and Collisions", 1, new Guid("8c039749-e2dc-12fc-0757-80e63a219052") },
                    { new Guid("1b255357-9b97-c844-146e-5cd377bcbc72"), "Government Agricultural Policies — Agriculture Grade 12", "Government Agricultural Policies", 9, new Guid("69e6f790-ae65-2e87-f351-982b99d92bfe") },
                    { new Guid("1d3c0e90-a4bd-3f3a-a005-543f4c69a357"), "Weather and Climate (Elements and Instruments) — Geography Grade 11", "Weather and Climate (Elements and Instruments)", 2, new Guid("629825b8-c2d2-7aee-d163-0be88495e272") },
                    { new Guid("20db5bed-020d-3921-3ba5-8dfafb75a3a0"), "Environmental Impact of Farming — Agriculture Grade 12", "Environmental Impact of Farming", 7, new Guid("69e6f790-ae65-2e87-f351-982b99d92bfe") },
                    { new Guid("2243f8e8-7d5c-4b81-a791-270127b09134"), "Alternating Current Circuits — Physics Grade 12", "Alternating Current Circuits", 2, new Guid("65ac38a6-5a9f-22e8-0de0-2181565e0d5e") },
                    { new Guid("2309e4b6-b2f4-252f-bcce-0dbfecc65395"), "Nutrition in Plants (Photosynthesis) — Biology Grade 10", "Nutrition in Plants (Photosynthesis)", 5, new Guid("6aaf47ef-e5a0-d4af-70db-f2011158ef8e") },
                    { new Guid("247ae126-cefe-56fd-0740-ee02fe42be6d"), "Livestock Management (Cattle, Poultry, Small Ruminants) — Agriculture Grade 11", "Livestock Management (Cattle, Poultry, Small Ruminants)", 5, new Guid("76324b9e-5fc2-7160-7522-932866c0a77a") },
                    { new Guid("250bbf2e-b484-c6b5-6223-a3d34c6284cf"), "Magnetic Fields and Forces — Physics Grade 12", "Magnetic Fields and Forces", 3, new Guid("65ac38a6-5a9f-22e8-0de0-2181565e0d5e") },
                    { new Guid("262bea99-c2e9-960c-7dda-cd1524692bdc"), "Scalars and Vectors — Physics Grade 10", "Scalars and Vectors", 2, new Guid("caea75ab-707a-872b-c92f-5500afab7afb") },
                    { new Guid("267e1777-1c7b-8c36-2e7e-5bb6be197426"), "Separation Techniques — Chemistry Grade 10", "Separation Techniques", 3, new Guid("45c2e4c9-1dad-eb06-8108-5a7b253db61d") },
                    { new Guid("2ac38533-14d1-0022-aab8-d2dce5ff5678"), "Introduction to Animal Husbandry — Agriculture Grade 10", "Introduction to Animal Husbandry", 10, new Guid("e37e03f0-c9ec-1714-3ff7-408956f8a8c0") },
                    { new Guid("2afea495-91b0-b821-9d70-9a348bd0ecdf"), "WASSCE Practical Chemistry Skills — Chemistry Grade 12", "WASSCE Practical Chemistry Skills", 9, new Guid("26a00ceb-e8ed-7c0b-b245-a60b6107d495") },
                    { new Guid("2bf61e36-db03-9b38-49a5-9ec1cc8091ca"), "Vectors in Two Dimensions — Mathematics Grade 12", "Vectors in Two Dimensions", 3, new Guid("eb63371f-c483-4b34-cf09-1f1f6825f8ba") },
                    { new Guid("2c669eb7-be1e-0265-0747-b2a1f1d6aec2"), "Water and Solutions — Chemistry Grade 10", "Water and Solutions", 8, new Guid("45c2e4c9-1dad-eb06-8108-5a7b253db61d") },
                    { new Guid("2d26137a-f0a3-f943-f2b9-e9116c28c34a"), "Transport in Living Things — Biology Grade 10", "Transport in Living Things", 7, new Guid("6aaf47ef-e5a0-d4af-70db-f2011158ef8e") },
                    { new Guid("2d54d1c2-d0c2-6c25-b6db-3ca57b4d634c"), "Irrigation and Water Management — Agriculture Grade 10", "Irrigation and Water Management", 8, new Guid("e37e03f0-c9ec-1714-3ff7-408956f8a8c0") },
                    { new Guid("2df790dc-9e24-927b-2853-ce14ed2ec462"), "Variation (Direct, Inverse, Joint, Partial) — Mathematics Grade 11", "Variation (Direct, Inverse, Joint, Partial)", 4, new Guid("5295e6aa-8238-0794-a9ec-29deb0bef2aa") },
                    { new Guid("2e6a92c5-a43d-4dbb-0ae1-d3224c004e31"), "Weathering and Erosion — Geography Grade 10", "Weathering and Erosion", 5, new Guid("f8380400-ac39-9946-15e0-894ba7e520e6") },
                    { new Guid("2fad72cf-9d7b-a3eb-6c75-a64d52804ebd"), "Organic Chemistry (Hydrocarbons) — Chemistry Grade 12", "Organic Chemistry (Hydrocarbons)", 1, new Guid("26a00ceb-e8ed-7c0b-b245-a60b6107d495") },
                    { new Guid("321d7703-ff2b-7967-3a47-cda216f3f097"), "WASSCE Past Questions Practice — Biology Grade 12", "WASSCE Past Questions Practice", 11, new Guid("06216eaa-a8e4-6710-e535-da8f3f9af209") },
                    { new Guid("32f6bc54-3d3d-2f1c-eb17-c2b6e6c18e97"), "Probability (Combined Events) — Mathematics Grade 11", "Probability (Combined Events)", 10, new Guid("5295e6aa-8238-0794-a9ec-29deb0bef2aa") },
                    { new Guid("36633574-50b2-eb6a-aa48-661184f0c770"), "Meaning and Importance of Agriculture — Agriculture Grade 10", "Meaning and Importance of Agriculture", 1, new Guid("e37e03f0-c9ec-1714-3ff7-408956f8a8c0") },
                    { new Guid("372c5f8f-3246-ef1f-af04-37cf9acc2fa0"), "Population (Distribution, Density, Growth) — Geography Grade 10", "Population (Distribution, Density, Growth)", 7, new Guid("f8380400-ac39-9946-15e0-894ba7e520e6") },
                    { new Guid("395bf01f-437b-e274-a53c-1925d463165a"), "Machines (Levers, Pulleys, Inclined Planes) — Physics Grade 11", "Machines (Levers, Pulleys, Inclined Planes)", 3, new Guid("8c039749-e2dc-12fc-0757-80e63a219052") },
                    { new Guid("3b46ce18-01f8-ca95-e113-2dd8475bb358"), "Agricultural Marketing and Trade — Agriculture Grade 12", "Agricultural Marketing and Trade", 4, new Guid("69e6f790-ae65-2e87-f351-982b99d92bfe") },
                    { new Guid("3dcd5dbb-56a4-8e14-be6e-d4477770b140"), "Pasture and Forage Crops — Agriculture Grade 12", "Pasture and Forage Crops", 2, new Guid("69e6f790-ae65-2e87-f351-982b99d92bfe") },
                    { new Guid("3dea0e34-b473-8f35-cd1a-142988b0c530"), "Variation and Evolution — Biology Grade 12", "Variation and Evolution", 2, new Guid("06216eaa-a8e4-6710-e535-da8f3f9af209") },
                    { new Guid("3f3e510d-4158-a222-8b3e-00e04ffc8594"), "Agricultural Mechanisation — Agriculture Grade 12", "Agricultural Mechanisation", 3, new Guid("69e6f790-ae65-2e87-f351-982b99d92bfe") },
                    { new Guid("416b09a4-43f2-eeda-0177-34ded7211777"), "Basic Trigonometry (Sine, Cosine, Tangent) — Mathematics Grade 10", "Basic Trigonometry (Sine, Cosine, Tangent)", 8, new Guid("f9d10be1-9d0d-14ec-37e4-3ab4dfc611f8") },
                    { new Guid("4217512d-4d37-34e6-d21b-78600bfd4ee0"), "Fluids at Rest and in Motion — Physics Grade 11", "Fluids at Rest and in Motion", 5, new Guid("8c039749-e2dc-12fc-0757-80e63a219052") },
                    { new Guid("434662b3-c252-6dd7-8de6-02a517fd0eaf"), "Acids, Bases and Salts (Advanced) — Chemistry Grade 11", "Acids, Bases and Salts (Advanced)", 6, new Guid("f71b0725-2ab4-36b8-c08a-d709a40e16f4") },
                    { new Guid("44d12cfb-912a-070e-62ec-8b16aa7b40e0"), "Sets and Venn Diagrams — Mathematics Grade 11", "Sets and Venn Diagrams", 1, new Guid("5295e6aa-8238-0794-a9ec-29deb0bef2aa") },
                    { new Guid("4505ec5d-f9a6-cf61-ec8c-3be513694042"), "Crop Processing and Storage — Agriculture Grade 11", "Crop Processing and Storage", 3, new Guid("76324b9e-5fc2-7160-7522-932866c0a77a") },
                    { new Guid("4527328c-43e4-cf41-4b78-c714e95d23ce"), "Recognising Living Things — Biology Grade 10", "Recognising Living Things", 1, new Guid("6aaf47ef-e5a0-d4af-70db-f2011158ef8e") },
                    { new Guid("45306343-3793-48b2-1771-f25a88b64d3c"), "Population (Migration, Urbanisation) — Geography Grade 11", "Population (Migration, Urbanisation)", 5, new Guid("629825b8-c2d2-7aee-d163-0be88495e272") },
                    { new Guid("46921bd3-c7bb-d3c1-5b24-9f4bbb976c60"), "Manufacturing Industries — Geography Grade 11", "Manufacturing Industries", 7, new Guid("629825b8-c2d2-7aee-d163-0be88495e272") },
                    { new Guid("47d24a92-000c-6bea-7e4e-ba0d4e8d9477"), "Plant Morphology and Physiology — Agriculture Grade 10", "Plant Morphology and Physiology", 6, new Guid("e37e03f0-c9ec-1714-3ff7-408956f8a8c0") },
                    { new Guid("47f3bf90-47b1-f557-e8fc-fa715418f1c1"), "Map Reading (Scales, Symbols, Bearings) — Geography Grade 10", "Map Reading (Scales, Symbols, Bearings)", 2, new Guid("f8380400-ac39-9946-15e0-894ba7e520e6") },
                    { new Guid("47fefc59-14de-0e36-2cac-2a13b73be938"), "Ecology (Habitats, Biomes, Ecosystems) — Biology Grade 11", "Ecology (Habitats, Biomes, Ecosystems)", 1, new Guid("b862ea9a-d88a-6230-aeea-858a90d5a959") },
                    { new Guid("4821a5eb-2b0b-f282-bb81-9f21ea6c4492"), "Energy Flow and Nutrient Cycling — Biology Grade 11", "Energy Flow and Nutrient Cycling", 2, new Guid("b862ea9a-d88a-6230-aeea-858a90d5a959") },
                    { new Guid("48b17420-dd20-05ee-00d9-aff48057a2b6"), "Reproductive Systems in Plants — Biology Grade 11", "Reproductive Systems in Plants", 3, new Guid("b862ea9a-d88a-6230-aeea-858a90d5a959") },
                    { new Guid("49c91862-5372-ea78-1898-f2b00ae430d6"), "Functions and Relations — Mathematics Grade 11", "Functions and Relations", 5, new Guid("5295e6aa-8238-0794-a9ec-29deb0bef2aa") },
                    { new Guid("4adcf74f-f687-d127-2e22-33154066f865"), "Natural Hazards and Disasters — Geography Grade 12", "Natural Hazards and Disasters", 7, new Guid("7a475323-7df5-f613-6449-fcfad4461d7c") },
                    { new Guid("4cc1eede-c546-e911-e169-cbf75574c951"), "Fractions, Decimals and Approximations — Mathematics Grade 10", "Fractions, Decimals and Approximations", 2, new Guid("f9d10be1-9d0d-14ec-37e4-3ab4dfc611f8") },
                    { new Guid("5047f972-a25d-e62d-19fd-c2ea17703aae"), "Redox Reactions and IUPAC Nomenclature — Chemistry Grade 12", "Redox Reactions and IUPAC Nomenclature", 7, new Guid("26a00ceb-e8ed-7c0b-b245-a60b6107d495") },
                    { new Guid("536cd1a5-10e6-ebc9-2da0-7952b1455dcd"), "WASSCE Past Questions Practice — Chemistry Grade 12", "WASSCE Past Questions Practice", 10, new Guid("26a00ceb-e8ed-7c0b-b245-a60b6107d495") },
                    { new Guid("53c6db7e-3a53-5218-a7e3-ce08a39ddf94"), "GIS and Remote Sensing (Introduction) — Geography Grade 12", "GIS and Remote Sensing (Introduction)", 9, new Guid("7a475323-7df5-f613-6449-fcfad4461d7c") },
                    { new Guid("5469ee84-ff7b-25a1-9997-8a33a1efe159"), "Organic Chemistry (Polymers and Plastics) — Chemistry Grade 12", "Organic Chemistry (Polymers and Plastics)", 3, new Guid("26a00ceb-e8ed-7c0b-b245-a60b6107d495") },
                    { new Guid("57103e21-91f1-cc4c-1fa9-daef5de110f3"), "Pollution and Its Effects — Biology Grade 12", "Pollution and Its Effects", 5, new Guid("06216eaa-a8e4-6710-e535-da8f3f9af209") },
                    { new Guid("5729a2f3-212a-cd84-db61-c902de27cdab"), "WASSCE Past Questions Practice — Agriculture Grade 12", "WASSCE Past Questions Practice", 10, new Guid("69e6f790-ae65-2e87-f351-982b99d92bfe") },
                    { new Guid("582aadc0-afc6-f2e4-249b-c884c3d410aa"), "Heat Transfer (Conduction, Convection, Radiation) — Physics Grade 10", "Heat Transfer (Conduction, Convection, Radiation)", 8, new Guid("caea75ab-707a-872b-c92f-5500afab7afb") },
                    { new Guid("58e6231b-500e-19ef-edc7-4d935d06a3eb"), "Map Reading (Contours, Cross-Sections) — Geography Grade 11", "Map Reading (Contours, Cross-Sections)", 1, new Guid("629825b8-c2d2-7aee-d163-0be88495e272") },
                    { new Guid("5df77b2f-c443-270a-2563-889aa434c8ba"), "Matrices and Determinants — Mathematics Grade 12", "Matrices and Determinants", 2, new Guid("eb63371f-c483-4b34-cf09-1f1f6825f8ba") },
                    { new Guid("5ee786fb-0ca2-4ac2-11f7-98004763d7a0"), "Energy Changes in Reactions (Enthalpy) — Chemistry Grade 11", "Energy Changes in Reactions (Enthalpy)", 3, new Guid("f71b0725-2ab4-36b8-c08a-d709a40e16f4") },
                    { new Guid("622e7382-a000-1393-8271-31c82f17a945"), "Nutrition in Animals — Biology Grade 10", "Nutrition in Animals", 6, new Guid("6aaf47ef-e5a0-d4af-70db-f2011158ef8e") },
                    { new Guid("659a4f4c-3338-d8e8-672d-cd63af50be90"), "Statistics (Data Collection, Mean, Median, Mode) — Mathematics Grade 10", "Statistics (Data Collection, Mean, Median, Mode)", 9, new Guid("f9d10be1-9d0d-14ec-37e4-3ab4dfc611f8") },
                    { new Guid("666852fb-a03a-5d58-420a-0a229adc19d5"), "Organic Chemistry (Functional Groups) — Chemistry Grade 12", "Organic Chemistry (Functional Groups)", 2, new Guid("26a00ceb-e8ed-7c0b-b245-a60b6107d495") },
                    { new Guid("683e25b0-54eb-916b-40c7-9100f226e0a6"), "Climate and Vegetation of West Africa — Geography Grade 10", "Climate and Vegetation of West Africa", 6, new Guid("f8380400-ac39-9946-15e0-894ba7e520e6") },
                    { new Guid("68c0a229-7cc2-494a-1797-de74b5842cc6"), "Chemical Formulae and Equations — Chemistry Grade 10", "Chemical Formulae and Equations", 6, new Guid("45c2e4c9-1dad-eb06-8108-5a7b253db61d") },
                    { new Guid("6959e07f-d270-8a51-f451-a0a391a3d9c2"), "Coordination and Control (Nervous System) — Biology Grade 11", "Coordination and Control (Nervous System)", 6, new Guid("b862ea9a-d88a-6230-aeea-858a90d5a959") },
                    { new Guid("6f104f94-0099-d7fd-cefa-199df5e4f701"), "Statistics (Frequency Distributions, Histograms) — Mathematics Grade 11", "Statistics (Frequency Distributions, Histograms)", 9, new Guid("5295e6aa-8238-0794-a9ec-29deb0bef2aa") },
                    { new Guid("6f6059b3-4563-9016-f01a-fe7e9ef0cc3c"), "Food Security and Safety in West Africa — Agriculture Grade 12", "Food Security and Safety in West Africa", 8, new Guid("69e6f790-ae65-2e87-f351-982b99d92bfe") },
                    { new Guid("6f967ce0-da0e-21c2-518e-6f0848d13f47"), "Metals and Their Compounds — Chemistry Grade 11", "Metals and Their Compounds", 8, new Guid("f71b0725-2ab4-36b8-c08a-d709a40e16f4") },
                    { new Guid("756abca4-8d6a-678a-21dd-ea4d60f17652"), "Radioactivity and Half-Life — Physics Grade 12", "Radioactivity and Half-Life", 7, new Guid("65ac38a6-5a9f-22e8-0de0-2181565e0d5e") },
                    { new Guid("75e57399-2f30-ca66-e270-b37110e9a00d"), "Ecology and Conservation of Natural Resources — Biology Grade 12", "Ecology and Conservation of Natural Resources", 4, new Guid("06216eaa-a8e4-6710-e535-da8f3f9af209") },
                    { new Guid("76b93960-e64a-8597-604a-ad92b0bf4dfc"), "Biology and Human Welfare — Biology Grade 12", "Biology and Human Welfare", 9, new Guid("06216eaa-a8e4-6710-e535-da8f3f9af209") },
                    { new Guid("780ad161-7c59-b149-9bdb-dfc5b34ec40e"), "Hormonal Coordination (Endocrine System) — Biology Grade 11", "Hormonal Coordination (Endocrine System)", 7, new Guid("b862ea9a-d88a-6230-aeea-858a90d5a959") },
                    { new Guid("78de959e-0333-6bd8-cc4f-2dd4d1ca71ce"), "Rocks and Minerals — Geography Grade 10", "Rocks and Minerals", 4, new Guid("f8380400-ac39-9946-15e0-894ba7e520e6") },
                    { new Guid("7c3016d9-09b6-7848-d994-38b0e6a91d79"), "Electrolysis and Electrochemistry — Chemistry Grade 11", "Electrolysis and Electrochemistry", 2, new Guid("f71b0725-2ab4-36b8-c08a-d709a40e16f4") },
                    { new Guid("7c8a5da9-2a50-aa33-0168-ad1b2ed25967"), "Electric Cells and Energy — Physics Grade 12", "Electric Cells and Energy", 4, new Guid("65ac38a6-5a9f-22e8-0de0-2181565e0d5e") },
                    { new Guid("7cb68f96-3b32-8ddb-28bd-89abd81d1add"), "Agricultural Economics (Demand, Supply, Pricing) — Agriculture Grade 11", "Agricultural Economics (Demand, Supply, Pricing)", 7, new Guid("76324b9e-5fc2-7160-7522-932866c0a77a") },
                    { new Guid("7f21b63d-70c7-c26f-27cd-f689cf7a2e90"), "Excretion — Biology Grade 10", "Excretion", 9, new Guid("6aaf47ef-e5a0-d4af-70db-f2011158ef8e") },
                    { new Guid("8189ad90-b599-d14b-d71f-c5af5846b535"), "Motion (Speed, Velocity, Acceleration) — Physics Grade 10", "Motion (Speed, Velocity, Acceleration)", 3, new Guid("caea75ab-707a-872b-c92f-5500afab7afb") },
                    { new Guid("81d96f62-d8c5-33bc-f8f2-68936519bde5"), "Reproductive Systems in Animals — Biology Grade 11", "Reproductive Systems in Animals", 4, new Guid("b862ea9a-d88a-6230-aeea-858a90d5a959") },
                    { new Guid("831f435a-ee19-aa98-74b6-dc983aefc71c"), "Environmental Chemistry (Pollution) — Chemistry Grade 12", "Environmental Chemistry (Pollution)", 5, new Guid("26a00ceb-e8ed-7c0b-b245-a60b6107d495") },
                    { new Guid("8457c068-bdf2-d993-e55b-3bef3ebfdae2"), "Chemical Equilibrium — Chemistry Grade 11", "Chemical Equilibrium", 5, new Guid("f71b0725-2ab4-36b8-c08a-d709a40e16f4") },
                    { new Guid("8495506f-1f7f-a2eb-ef3e-c81708fef232"), "Agricultural Extension Services — Agriculture Grade 11", "Agricultural Extension Services", 9, new Guid("76324b9e-5fc2-7160-7522-932866c0a77a") },
                    { new Guid("8c53306e-2816-61a1-87d7-b77ba83bff14"), "Adaptation of Organisms — Biology Grade 12", "Adaptation of Organisms", 6, new Guid("06216eaa-a8e4-6710-e535-da8f3f9af209") },
                    { new Guid("8da30564-2e73-adc8-3d41-33646d06632f"), "Biotechnology and Genetic Engineering — Biology Grade 12", "Biotechnology and Genetic Engineering", 8, new Guid("06216eaa-a8e4-6710-e535-da8f3f9af209") },
                    { new Guid("8dc98e0c-1145-2d51-5871-c4ba49f97d95"), "Weed Management — Agriculture Grade 11", "Weed Management", 2, new Guid("76324b9e-5fc2-7160-7522-932866c0a77a") },
                    { new Guid("8e7863c6-9496-baae-1184-be68e2c2e7f5"), "Diseases and Immunology — Biology Grade 11", "Diseases and Immunology", 10, new Guid("b862ea9a-d88a-6230-aeea-858a90d5a959") },
                    { new Guid("90996712-c7c1-4dea-0d28-836eab346490"), "Capacitance and Capacitors — Physics Grade 11", "Capacitance and Capacitors", 9, new Guid("8c039749-e2dc-12fc-0757-80e63a219052") },
                    { new Guid("90cb99a1-80a3-f69e-36bc-a0e4db229973"), "Geomorphic Processes (Rivers, Coasts, Wind) — Geography Grade 11", "Geomorphic Processes (Rivers, Coasts, Wind)", 3, new Guid("629825b8-c2d2-7aee-d163-0be88495e272") },
                    { new Guid("962beebd-0018-45fb-27e5-b9e26907b876"), "Probability (Basic Concepts) — Mathematics Grade 10", "Probability (Basic Concepts)", 10, new Guid("f9d10be1-9d0d-14ec-37e4-3ab4dfc611f8") },
                    { new Guid("96d8ec76-9859-2e70-76a7-dd83cb195745"), "Current Electricity (Ohm's Law, Circuits) — Physics Grade 11", "Current Electricity (Ohm's Law, Circuits)", 10, new Guid("8c039749-e2dc-12fc-0757-80e63a219052") },
                    { new Guid("9871ca45-cf53-3d18-3cd9-f558d029a1c2"), "Introduction to Chemistry (Laboratory Safety) — Chemistry Grade 10", "Introduction to Chemistry (Laboratory Safety)", 1, new Guid("45c2e4c9-1dad-eb06-8108-5a7b253db61d") },
                    { new Guid("9cbe385c-c375-590f-fcb0-6248c01b0a67"), "Soil Fertility and Conservation — Agriculture Grade 10", "Soil Fertility and Conservation", 4, new Guid("e37e03f0-c9ec-1714-3ff7-408956f8a8c0") },
                    { new Guid("9d999069-8b08-4546-ab87-9276d8b83496"), "Coordinate Geometry (Straight Lines) — Mathematics Grade 11", "Coordinate Geometry (Straight Lines)", 6, new Guid("5295e6aa-8238-0794-a9ec-29deb0bef2aa") },
                    { new Guid("9e05d904-b4c8-8c19-a32d-3664443b5d05"), "Electrostatics (Charges, Electric Fields) — Physics Grade 11", "Electrostatics (Charges, Electric Fields)", 8, new Guid("8c039749-e2dc-12fc-0757-80e63a219052") },
                    { new Guid("9eba9460-c2dc-ed90-c223-908912166a05"), "Algebraic Expressions and Factorisation — Mathematics Grade 10", "Algebraic Expressions and Factorisation", 4, new Guid("f9d10be1-9d0d-14ec-37e4-3ab4dfc611f8") },
                    { new Guid("a0b59b38-0b24-6e0e-8bc0-90a44457c6c8"), "Types of Agriculture in The Gambia — Agriculture Grade 10", "Types of Agriculture in The Gambia", 2, new Guid("e37e03f0-c9ec-1714-3ff7-408956f8a8c0") },
                    { new Guid("a0d9ba4e-6165-1d95-981a-cccc316a8b14"), "Genetics (Mendel's Laws, Inheritance Patterns) — Biology Grade 12", "Genetics (Mendel's Laws, Inheritance Patterns)", 1, new Guid("06216eaa-a8e4-6710-e535-da8f3f9af209") },
                    { new Guid("a502c8bf-0c86-7ce7-346f-2c5f86c37502"), "Carbon and Its Compounds (Organic Chemistry Intro) — Chemistry Grade 11", "Carbon and Its Compounds (Organic Chemistry Intro)", 7, new Guid("f71b0725-2ab4-36b8-c08a-d709a40e16f4") },
                    { new Guid("a50e398c-9073-f304-4f07-1d9051d757a9"), "Settlement Types and Patterns — Geography Grade 10", "Settlement Types and Patterns", 8, new Guid("f8380400-ac39-9946-15e0-894ba7e520e6") },
                    { new Guid("a77e69bd-7999-b09a-892f-433925bda0f3"), "Landforms and Drainage — Geography Grade 10", "Landforms and Drainage", 3, new Guid("f8380400-ac39-9946-15e0-894ba7e520e6") },
                    { new Guid("a7c1171f-8023-49c7-4a20-a9e3988986db"), "Soil Science (Formation, Types, Profiles) — Agriculture Grade 10", "Soil Science (Formation, Types, Profiles)", 3, new Guid("e37e03f0-c9ec-1714-3ff7-408956f8a8c0") },
                    { new Guid("a7c91f27-5cc6-8de5-85f9-5eb83eeb52c6"), "Thermal Energy and Temperature — Physics Grade 10", "Thermal Energy and Temperature", 7, new Guid("caea75ab-707a-872b-c92f-5500afab7afb") },
                    { new Guid("a7d4d5d0-afcd-5f7f-c2c7-d0d5891073a3"), "Quantitative Analysis (Titrations) — Chemistry Grade 12", "Quantitative Analysis (Titrations)", 8, new Guid("26a00ceb-e8ed-7c0b-b245-a60b6107d495") },
                    { new Guid("aa0d88ac-a24c-057f-7554-80c39d2b306b"), "Farm Management and Business Planning — Agriculture Grade 12", "Farm Management and Business Planning", 5, new Guid("69e6f790-ae65-2e87-f351-982b99d92bfe") },
                    { new Guid("aaec6b71-a067-cb18-27df-4540268e4e81"), "Map Reading (Advanced Interpretation) — Geography Grade 12", "Map Reading (Advanced Interpretation)", 1, new Guid("7a475323-7df5-f613-6449-fcfad4461d7c") },
                    { new Guid("adcc06f6-0c6c-28b3-fd04-733aafa6883d"), "Respiratory System — Biology Grade 10", "Respiratory System", 8, new Guid("6aaf47ef-e5a0-d4af-70db-f2011158ef8e") },
                    { new Guid("b00fa90c-01d3-c068-aa42-6b547e58fc77"), "Equilibrium of Forces (Moments, Couples) — Physics Grade 11", "Equilibrium of Forces (Moments, Couples)", 2, new Guid("8c039749-e2dc-12fc-0757-80e63a219052") },
                    { new Guid("b0a7c71a-711c-9ef0-6be6-6ef20b546a26"), "Sequences and Series (AP, GP) — Mathematics Grade 12", "Sequences and Series (AP, GP)", 1, new Guid("eb63371f-c483-4b34-cf09-1f1f6825f8ba") },
                    { new Guid("b0bd56d8-19b3-e4ce-9f22-54127f3452c6"), "Mining and Power Resources — Geography Grade 11", "Mining and Power Resources", 8, new Guid("629825b8-c2d2-7aee-d163-0be88495e272") },
                    { new Guid("b0cae5e7-4a34-5c81-5e53-949dd3e3f7ec"), "Cell Structure and Organisation — Biology Grade 10", "Cell Structure and Organisation", 2, new Guid("6aaf47ef-e5a0-d4af-70db-f2011158ef8e") },
                    { new Guid("b14ad301-413e-6eff-ebb8-38f9b2a97c0d"), "Indices and Logarithms — Mathematics Grade 10", "Indices and Logarithms", 3, new Guid("f9d10be1-9d0d-14ec-37e4-3ab4dfc611f8") },
                    { new Guid("b50eeeff-b0ad-c1ec-1920-b45731f69137"), "Tourism in The Gambia and West Africa — Geography Grade 11", "Tourism in The Gambia and West Africa", 10, new Guid("629825b8-c2d2-7aee-d163-0be88495e272") },
                    { new Guid("b76c3ec8-eb6a-6ab6-f3a4-c9ada0006b09"), "Factors Affecting Living Organisms — Biology Grade 10", "Factors Affecting Living Organisms", 10, new Guid("6aaf47ef-e5a0-d4af-70db-f2011158ef8e") },
                    { new Guid("bb6f0378-ce5e-32db-e0c9-a89b51413ab7"), "Forestry and Agroforestry — Agriculture Grade 11", "Forestry and Agroforestry", 10, new Guid("76324b9e-5fc2-7160-7522-932866c0a77a") },
                    { new Guid("bd8072f1-fd0a-4656-db4c-eeea4c88eb6e"), "The Mole Concept and Stoichiometry — Chemistry Grade 11", "The Mole Concept and Stoichiometry", 1, new Guid("f71b0725-2ab4-36b8-c08a-d709a40e16f4") },
                    { new Guid("be605b33-0e65-58db-9b7d-335a37cb5d9b"), "Acids, Bases and Salts (Introduction) — Chemistry Grade 10", "Acids, Bases and Salts (Introduction)", 9, new Guid("45c2e4c9-1dad-eb06-8108-5a7b253db61d") },
                    { new Guid("be6e0366-42c6-8cd0-9791-4d4469a17540"), "Causes of Motion (Newton's Laws) — Physics Grade 10", "Causes of Motion (Newton's Laws)", 4, new Guid("caea75ab-707a-872b-c92f-5500afab7afb") },
                    { new Guid("be8b17a0-aaa6-5d2e-4ba0-3ecd2d6c8237"), "Globalisation and Development — Geography Grade 12", "Globalisation and Development", 8, new Guid("7a475323-7df5-f613-6449-fcfad4461d7c") },
                    { new Guid("bec7edf6-fc77-d145-920b-9ab648e784b8"), "Sense Organs (Eye, Ear) — Biology Grade 11", "Sense Organs (Eye, Ear)", 8, new Guid("b862ea9a-d88a-6230-aeea-858a90d5a959") },
                    { new Guid("bfe4c3f0-a695-5bee-0ca5-c709fc62e845"), "Waves (Types and Properties) — Physics Grade 10", "Waves (Types and Properties)", 9, new Guid("caea75ab-707a-872b-c92f-5500afab7afb") },
                    { new Guid("c226ff6d-8b3e-a606-63d4-c61c9fea890c"), "Simple Equations and Inequalities — Mathematics Grade 10", "Simple Equations and Inequalities", 5, new Guid("f9d10be1-9d0d-14ec-37e4-3ab4dfc611f8") },
                    { new Guid("c3fb7d1b-108c-4101-8060-4f84fbd6a81c"), "Farm Records and Accounts — Agriculture Grade 11", "Farm Records and Accounts", 8, new Guid("76324b9e-5fc2-7160-7522-932866c0a77a") },
                    { new Guid("c43e963a-abae-969f-698b-808daf65ba87"), "Regional Geography of West Africa — Geography Grade 12", "Regional Geography of West Africa", 4, new Guid("7a475323-7df5-f613-6449-fcfad4461d7c") },
                    { new Guid("c44a7729-fe7f-6ce7-9adf-236ea00d79db"), "Farm Tools and Implements — Agriculture Grade 10", "Farm Tools and Implements", 5, new Guid("e37e03f0-c9ec-1714-3ff7-408956f8a8c0") },
                    { new Guid("c48e55eb-c3f4-05df-6b62-f5c59a950ac0"), "Chemical Reactions (Types) — Chemistry Grade 10", "Chemical Reactions (Types)", 10, new Guid("45c2e4c9-1dad-eb06-8108-5a7b253db61d") },
                    { new Guid("c5389e79-0f83-f835-ba36-4dfb4b637790"), "Crop Production (Land Preparation, Planting) — Agriculture Grade 10", "Crop Production (Land Preparation, Planting)", 7, new Guid("e37e03f0-c9ec-1714-3ff7-408956f8a8c0") },
                    { new Guid("c672d2e8-fae8-9eed-3a44-ea5fbf0de465"), "Atomic and Nuclear Physics — Physics Grade 12", "Atomic and Nuclear Physics", 6, new Guid("65ac38a6-5a9f-22e8-0de0-2181565e0d5e") },
                    { new Guid("c6ab93ca-93b2-c527-74ed-57f57b363f89"), "Transportation and Communication — Geography Grade 11", "Transportation and Communication", 6, new Guid("629825b8-c2d2-7aee-d163-0be88495e272") },
                    { new Guid("c709ac91-b15e-39df-45e9-0ccafeb9ee8f"), "Vegetation Zones of Africa — Geography Grade 11", "Vegetation Zones of Africa", 4, new Guid("629825b8-c2d2-7aee-d163-0be88495e272") },
                    { new Guid("c95f3651-a0c5-94e7-fd59-8363838ccbf3"), "Introductory Electronics (Diodes, Transistors) — Physics Grade 12", "Introductory Electronics (Diodes, Transistors)", 5, new Guid("65ac38a6-5a9f-22e8-0de0-2181565e0d5e") },
                    { new Guid("ca5bb240-ff3f-8fd7-82fa-db86f34ac516"), "Classification of Living Things — Biology Grade 10", "Classification of Living Things", 4, new Guid("6aaf47ef-e5a0-d4af-70db-f2011158ef8e") },
                    { new Guid("ca8c5c8a-5b3a-3b3b-28a5-1c3e651a7c98"), "Environmental Conservation and Management — Geography Grade 12", "Environmental Conservation and Management", 6, new Guid("7a475323-7df5-f613-6449-fcfad4461d7c") },
                    { new Guid("cbaaa277-dba2-648d-f113-59fb0fa6ca42"), "Atomic Structure and the Periodic Table — Chemistry Grade 10", "Atomic Structure and the Periodic Table", 4, new Guid("45c2e4c9-1dad-eb06-8108-5a7b253db61d") },
                    { new Guid("ccd9ccc6-a02b-0ae1-9b8e-1217fd227ec7"), "WASSCE Map Work and Past Questions Practice — Geography Grade 12", "WASSCE Map Work and Past Questions Practice", 10, new Guid("7a475323-7df5-f613-6449-fcfad4461d7c") },
                    { new Guid("cd37d1c0-4098-944a-0d2b-243277c2b52a"), "Work, Energy and Power — Physics Grade 10", "Work, Energy and Power", 5, new Guid("caea75ab-707a-872b-c92f-5500afab7afb") },
                    { new Guid("cdeb60b9-6582-3e3b-b9a7-b8e432ac84b5"), "Construction and Loci — Mathematics Grade 12", "Construction and Loci", 7, new Guid("eb63371f-c483-4b34-cf09-1f1f6825f8ba") },
                    { new Guid("d0a9e4f2-e994-203b-4b0a-02a17d3d9e4d"), "Agriculture in The Gambia and West Africa — Geography Grade 10", "Agriculture in The Gambia and West Africa", 9, new Guid("f8380400-ac39-9946-15e0-894ba7e520e6") },
                    { new Guid("d12e24f4-addf-493c-bf24-ed72b3179eb9"), "Probability (Permutations, Combinations) — Mathematics Grade 12", "Probability (Permutations, Combinations)", 10, new Guid("eb63371f-c483-4b34-cf09-1f1f6825f8ba") },
                    { new Guid("d36954d4-150f-419e-89f7-2d4c69e745e0"), "Chemical Bonding (Ionic, Covalent, Metallic) — Chemistry Grade 10", "Chemical Bonding (Ionic, Covalent, Metallic)", 5, new Guid("45c2e4c9-1dad-eb06-8108-5a7b253db61d") },
                    { new Guid("d80bbf4d-a28d-5ae7-b780-bfee2d03ef31"), "Elasticity (Hooke's Law) — Physics Grade 11", "Elasticity (Hooke's Law)", 4, new Guid("8c039749-e2dc-12fc-0757-80e63a219052") },
                    { new Guid("d853e542-d37f-35ec-6637-f0fd3cd18946"), "WASSCE Practical Biology Skills — Biology Grade 12", "WASSCE Practical Biology Skills", 10, new Guid("06216eaa-a8e4-6710-e535-da8f3f9af209") },
                    { new Guid("db7be8f9-9d77-06c2-61e7-4a529afa984d"), "Crop Improvement and Seed Technology — Agriculture Grade 11", "Crop Improvement and Seed Technology", 1, new Guid("76324b9e-5fc2-7160-7522-932866c0a77a") },
                    { new Guid("dbcb578c-5ceb-fa42-e3f9-5e8fa1ba5fbb"), "DNA, Genes and Chromosomes — Biology Grade 12", "DNA, Genes and Chromosomes", 7, new Guid("06216eaa-a8e4-6710-e535-da8f3f9af209") },
                    { new Guid("dcd2c783-c8c7-6bbc-8a3e-edbca1cb89eb"), "Circle Theorems — Mathematics Grade 12", "Circle Theorems", 6, new Guid("eb63371f-c483-4b34-cf09-1f1f6825f8ba") },
                    { new Guid("dd8a8eb2-560b-b1a4-3df3-af9df6344dc3"), "Mensuration (Circles, Sectors, Arcs) — Mathematics Grade 11", "Mensuration (Circles, Sectors, Arcs)", 8, new Guid("5295e6aa-8238-0794-a9ec-29deb0bef2aa") },
                    { new Guid("de185619-221c-bc79-a90a-4f11ecc89c84"), "Animal Health and Disease Control — Agriculture Grade 11", "Animal Health and Disease Control", 6, new Guid("76324b9e-5fc2-7160-7522-932866c0a77a") },
                    { new Guid("e00193f5-2dd9-b11c-4a04-cfe494939732"), "Statistics (Cumulative Frequency, Quartiles) — Mathematics Grade 12", "Statistics (Cumulative Frequency, Quartiles)", 9, new Guid("eb63371f-c483-4b34-cf09-1f1f6825f8ba") },
                    { new Guid("e15ea2f8-eac6-d915-2ec2-d23c81666150"), "Trigonometry (Sine and Cosine Rules) — Mathematics Grade 12", "Trigonometry (Sine and Cosine Rules)", 5, new Guid("eb63371f-c483-4b34-cf09-1f1f6825f8ba") },
                    { new Guid("e17d9891-fd78-58bf-4cd9-6653532f15b9"), "Light (Reflection, Refraction, Lenses) — Physics Grade 11", "Light (Reflection, Refraction, Lenses)", 6, new Guid("8c039749-e2dc-12fc-0757-80e63a219052") },
                    { new Guid("e1f51036-cbfc-9015-a3da-94c549f1e969"), "Particulate Nature of Matter — Chemistry Grade 10", "Particulate Nature of Matter", 2, new Guid("45c2e4c9-1dad-eb06-8108-5a7b253db61d") },
                    { new Guid("e4b60c45-6fa9-1111-744d-e193533565db"), "Wave-Particle Duality — Physics Grade 12", "Wave-Particle Duality", 8, new Guid("65ac38a6-5a9f-22e8-0de0-2181565e0d5e") },
                    { new Guid("e6e67b46-ebb3-9e9b-00e4-397ed193a1c8"), "Measurements and Units — Physics Grade 10", "Measurements and Units", 1, new Guid("caea75ab-707a-872b-c92f-5500afab7afb") },
                    { new Guid("e98af30f-5ffa-b438-5f86-bccfd6f21899"), "Quadratic Equations — Mathematics Grade 11", "Quadratic Equations", 2, new Guid("5295e6aa-8238-0794-a9ec-29deb0bef2aa") },
                    { new Guid("e9c21c4b-507d-b91a-cd0a-69ccbe6246b2"), "Rates of Chemical Reactions — Chemistry Grade 11", "Rates of Chemical Reactions", 4, new Guid("f71b0725-2ab4-36b8-c08a-d709a40e16f4") },
                    { new Guid("ea16fa76-8f98-d05f-126b-fb1836257b54"), "Optical Instruments — Physics Grade 11", "Optical Instruments", 7, new Guid("8c039749-e2dc-12fc-0757-80e63a219052") },
                    { new Guid("eb582ba4-a9a5-1504-96e9-ac95e265b1f9"), "Environmental Problems — Geography Grade 10", "Environmental Problems", 10, new Guid("f8380400-ac39-9946-15e0-894ba7e520e6") },
                    { new Guid("eb7878d3-2091-e755-462e-0f9c2a349865"), "Geometry (Angles, Triangles, Polygons) — Mathematics Grade 10", "Geometry (Angles, Triangles, Polygons)", 6, new Guid("f9d10be1-9d0d-14ec-37e4-3ab4dfc611f8") },
                    { new Guid("eff9b4f3-c029-d06b-3808-479be99fb66e"), "Economic Activities of Major World Regions — Geography Grade 12", "Economic Activities of Major World Regions", 3, new Guid("7a475323-7df5-f613-6449-fcfad4461d7c") },
                    { new Guid("f08994af-5f8b-cf6e-9e0c-3149bce9c19b"), "Pressure (Solids, Liquids, Gases) — Physics Grade 10", "Pressure (Solids, Liquids, Gases)", 6, new Guid("caea75ab-707a-872b-c92f-5500afab7afb") },
                    { new Guid("f2e774dc-376b-2d70-0959-7951d08caad3"), "Meiosis and Genetic Variation — Biology Grade 12", "Meiosis and Genetic Variation", 3, new Guid("06216eaa-a8e4-6710-e535-da8f3f9af209") },
                    { new Guid("f3260204-f771-3cdb-67cc-18f16b4941e8"), "Agricultural Credit and Cooperatives — Agriculture Grade 12", "Agricultural Credit and Cooperatives", 6, new Guid("69e6f790-ae65-2e87-f351-982b99d92bfe") },
                    { new Guid("f65eb5a5-7fcb-794d-ea27-074d6eabd45d"), "WASSCE Practical Physics Skills — Physics Grade 12", "WASSCE Practical Physics Skills", 10, new Guid("65ac38a6-5a9f-22e8-0de0-2181565e0d5e") },
                    { new Guid("f6e7e4ad-5fe7-b9f2-8d73-d8bbeb918287"), "Animal Reproduction and Breeding — Agriculture Grade 12", "Animal Reproduction and Breeding", 1, new Guid("69e6f790-ae65-2e87-f351-982b99d92bfe") },
                    { new Guid("f71d5405-b478-a735-bb9f-27119c097bcd"), "Electromagnetic Induction (Faraday's Law) — Physics Grade 12", "Electromagnetic Induction (Faraday's Law)", 1, new Guid("65ac38a6-5a9f-22e8-0de0-2181565e0d5e") },
                    { new Guid("f77b8d01-6e58-b0cc-af73-720561a5b83a"), "The Earth and the Solar System — Geography Grade 10", "The Earth and the Solar System", 1, new Guid("f8380400-ac39-9946-15e0-894ba7e520e6") },
                    { new Guid("f77dfc64-755e-b4db-85e3-b0962ec5edd3"), "States of Matter and Gas Laws — Chemistry Grade 10", "States of Matter and Gas Laws", 7, new Guid("45c2e4c9-1dad-eb06-8108-5a7b253db61d") },
                    { new Guid("f7b67f08-0cb4-8bda-52ef-ddb4919cd95b"), "Mensuration (Cones, Spheres, Pyramids) — Mathematics Grade 12", "Mensuration (Cones, Spheres, Pyramids)", 8, new Guid("eb63371f-c483-4b34-cf09-1f1f6825f8ba") },
                    { new Guid("f7ff93c4-c977-dd0d-17fe-f025e9654c13"), "Physical Geography of Africa and the World — Geography Grade 12", "Physical Geography of Africa and the World", 2, new Guid("7a475323-7df5-f613-6449-fcfad4461d7c") },
                    { new Guid("fb2f36ee-8b47-9219-8cf7-4000eb32a1b3"), "Trigonometry (Graphs, Identities, Equations) — Mathematics Grade 11", "Trigonometry (Graphs, Identities, Equations)", 7, new Guid("5295e6aa-8238-0794-a9ec-29deb0bef2aa") },
                    { new Guid("fc0ce933-eb82-02be-d378-aacec0fc3963"), "Nuclear Chemistry — Chemistry Grade 12", "Nuclear Chemistry", 6, new Guid("26a00ceb-e8ed-7c0b-b245-a60b6107d495") },
                    { new Guid("fd3dbd5b-b789-7cb4-23f6-bd9fd9c3e828"), "Trade (Internal and International) — Geography Grade 11", "Trade (Internal and International)", 9, new Guid("629825b8-c2d2-7aee-d163-0be88495e272") },
                    { new Guid("fdff7b44-f5d8-d9de-a9fc-d54ff383c2e6"), "The Gambia — Physical and Economic Geography — Geography Grade 12", "The Gambia — Physical and Economic Geography", 5, new Guid("7a475323-7df5-f613-6449-fcfad4461d7c") },
                    { new Guid("00a8e277-4667-1f0e-4897-991d0a4c4d03"), "Faith and Works (James, Hebrews) — Christian Religious Studies Grade 12", "Faith and Works (James, Hebrews)", 3, new Guid("30f59780-4140-3f0a-cded-8b13fd7052cb") },
                    { new Guid("0116c6ed-8307-dbf2-8b29-12c8e1a89930"), "International Trade (Theory, Barriers, Agreements) — Economics Grade 12", "International Trade (Theory, Barriers, Agreements)", 1, new Guid("0d331ad8-0343-b862-4378-b5f017817a63") },
                    { new Guid("0194dbbf-5c9e-ae66-bb6d-99a456e37faf"), "Stock Valuation (FIFO, LIFO, AVCO) — Financial Accounting Grade 12", "Stock Valuation (FIFO, LIFO, AVCO)", 3, new Guid("4d512f11-4373-8e2f-543c-0842a91f4cf2") },
                    { new Guid("040a6bb2-ac5d-1c7c-2272-f696027fde74"), "Fruit Production (Tropical Fruits) — Horticulture Grade 10", "Fruit Production (Tropical Fruits)", 7, new Guid("3c22945c-cc6c-6199-f614-746cb565217c") },
                    { new Guid("04371f1e-30fd-43f7-3a04-28df28a1e0a8"), "Floriculture Business Management — Floriculture Grade 12", "Floriculture Business Management", 4, new Guid("f1ddfb49-3ee7-d0d0-3bdb-ba71e5d13aec") },
                    { new Guid("0446a6b9-f4d1-95d2-c483-55480dca9fbd"), "Pan-Africanism and the OAU/AU — History Grade 11", "Pan-Africanism and the OAU/AU", 7, new Guid("a33a76d9-ae1b-fb53-fef6-83cfc12992bb") },
                    { new Guid("0451e2ef-ac0a-00ba-5d05-9c5aa92b1b9b"), "Fishing Gear and Methods — Fisheries Grade 10", "Fishing Gear and Methods", 7, new Guid("70a66f3c-d5f9-1b35-bc9c-119ef3d85f3e") },
                    { new Guid("052f877a-a0df-a8f3-a8c5-93c70d19324e"), "Distributive Trade — Economics Grade 11", "Distributive Trade", 8, new Guid("04fbf1c3-6b48-d6c9-e60b-8af777b7fa50") },
                    { new Guid("054d1791-1ad3-23c2-e615-b1eddb658629"), "Accounting Ratios and Interpretation — Financial Accounting Grade 11", "Accounting Ratios and Interpretation", 9, new Guid("c33eef6f-8e20-9183-f5a2-4e5187de42d7") },
                    { new Guid("076e9f62-e47c-b300-a3d1-d91a65210d69"), "Auditing Basics — Financial Accounting Grade 12", "Auditing Basics", 9, new Guid("4d512f11-4373-8e2f-543c-0842a91f4cf2") },
                    { new Guid("079581b0-0fea-f36c-7275-48b44f8ed172"), "Spice and Medicinal Crop Production — Horticulture Grade 11", "Spice and Medicinal Crop Production", 5, new Guid("efa84916-91b0-0dea-73a8-860fc9a80941") },
                    { new Guid("07b85e17-acfc-c650-6dae-f92579e11ed3"), "Cut Flower Production (Rose, Chrysanthemum, Carnation) — Floriculture Grade 11", "Cut Flower Production (Rose, Chrysanthemum, Carnation)", 1, new Guid("49841f61-c212-8eec-2810-f1bb824e37d2") },
                    { new Guid("08c49636-77de-23a1-404e-e1185aa9316f"), "Published Accounts and Financial Statements — Financial Accounting Grade 12", "Published Accounts and Financial Statements", 2, new Guid("4d512f11-4373-8e2f-543c-0842a91f4cf2") },
                    { new Guid("08fd6d19-ad75-86f6-9474-8d5a85c74aff"), "Christians and the State — Christian Religious Studies Grade 11", "Christians and the State", 10, new Guid("97ec5abf-b57f-0843-fe64-8f31af424c93") },
                    { new Guid("0a4f38bd-1221-8ff4-840a-ad4fff3f0489"), "Advanced Aquaculture Techniques — Fisheries Grade 12", "Advanced Aquaculture Techniques", 1, new Guid("aea3fc29-a3f7-76c0-8be7-f73335d78889") },
                    { new Guid("0ac76175-e7b0-3395-f338-cc6dfe970979"), "Single Entry and Incomplete Records — Financial Accounting Grade 11", "Single Entry and Incomplete Records", 6, new Guid("c33eef6f-8e20-9183-f5a2-4e5187de42d7") },
                    { new Guid("0c3635a2-378b-685e-2637-41059b33ac57"), "Depreciation of Fixed Assets — Financial Accounting Grade 11", "Depreciation of Fixed Assets", 2, new Guid("c33eef6f-8e20-9183-f5a2-4e5187de42d7") },
                    { new Guid("0cc1a321-2df8-fc3a-48de-59db7b61e3ee"), "Consumer Protection — Commerce Grade 11", "Consumer Protection", 6, new Guid("773db100-257e-8d22-3695-0e647cbb7fd4") },
                    { new Guid("0d86fa2f-9438-1bcf-24fc-20990b392592"), "Business Finance (Sources of Capital) — Commerce Grade 11", "Business Finance (Sources of Capital)", 7, new Guid("773db100-257e-8d22-3695-0e647cbb7fd4") },
                    { new Guid("0dabe316-73a4-3582-eaea-25e25b65e916"), "The Miracles of Jesus — Christian Religious Studies Grade 11", "The Miracles of Jesus", 2, new Guid("97ec5abf-b57f-0843-fe64-8f31af424c93") },
                    { new Guid("0ecd35ca-efec-22c2-da69-caed52eddde0"), "The Constitution (Types, Features, Importance) — Government Grade 10", "The Constitution (Types, Features, Importance)", 6, new Guid("c0e3b77a-b7e6-f84c-1414-a9b4986364c7") },
                    { new Guid("0f4ee199-edbc-07ae-96de-7f3b2c67ff26"), "Climate and Soil Requirements for Flower Production — Floriculture Grade 10", "Climate and Soil Requirements for Flower Production", 4, new Guid("4b421fd5-92e6-ef02-edd7-7804412bedd0") },
                    { new Guid("10a84cf3-d0c4-4817-badf-6be1d3c58684"), "Marketing of Horticultural Produce — Horticulture Grade 11", "Marketing of Horticultural Produce", 10, new Guid("efa84916-91b0-0dea-73a8-860fc9a80941") },
                    { new Guid("10c1d385-d2e6-a97c-0819-7a25f0530bad"), "The Atlantic Slave Trade — History Grade 10", "The Atlantic Slave Trade", 5, new Guid("18c23abd-93c6-0e94-492c-dfd6d0a2ce28") },
                    { new Guid("10f7e24c-5f61-2b8d-e6bb-44632c401301"), "Comprehension (Advanced Inference and Deduction) — English Language Grade 11", "Comprehension (Advanced Inference and Deduction)", 1, new Guid("f081fc93-cdb6-c836-9743-149f52228aab") },
                    { new Guid("112f321d-6396-88b6-dfe5-9401888beef9"), "Muslim Scholars and Their Contributions — Islamic Religious Studies Grade 12", "Muslim Scholars and Their Contributions", 9, new Guid("0a0e509f-a99c-3193-c277-928d54224cc2") },
                    { new Guid("11bf59f4-6cc4-6e88-627e-f34a2220bb90"), "Surds and Indices (Advanced) — Further Mathematics Grade 10", "Surds and Indices (Advanced)", 2, new Guid("a4eaeeb3-b7a3-9abd-1c53-5309c8d473c0") },
                    { new Guid("12b1717e-7f07-27b0-2fe0-33e117531c53"), "The Umayyad and Abbasid Dynasties — Islamic Religious Studies Grade 12", "The Umayyad and Abbasid Dynasties", 1, new Guid("0a0e509f-a99c-3193-c277-928d54224cc2") },
                    { new Guid("138094d8-7dad-8c44-815f-5d6b4f47449f"), "Colonial Administration in West Africa — Government Grade 11", "Colonial Administration in West Africa", 7, new Guid("c5ad46a4-1f10-7aab-6b9d-9d32bca37e63") },
                    { new Guid("13bf3a9c-0d22-a404-12ba-c1aa8a70a36a"), "Letter Writing (Informal) — English Language Grade 10", "Letter Writing (Informal)", 7, new Guid("def2214a-a301-249e-c656-0914b4ecb5a5") },
                    { new Guid("14530323-56fe-b18b-5c71-46a86b3fb4f1"), "ICT and Society (Impact, Careers) — Computer Studies Grade 12", "ICT and Society (Impact, Careers)", 8, new Guid("30f1307e-bbbc-d420-61c7-124673a9fd1c") },
                    { new Guid("1577a74d-ccf6-b5f7-b4ba-070a3a805554"), "Web Design and Development (HTML, CSS) — Computer Studies Grade 12", "Web Design and Development (HTML, CSS)", 1, new Guid("30f1307e-bbbc-d420-61c7-124673a9fd1c") },
                    { new Guid("1699dad2-619a-7eab-cb01-23576193dc97"), "The Quran (Revelation, Compilation, Structure) — Islamic Religious Studies Grade 10", "The Quran (Revelation, Compilation, Structure)", 2, new Guid("598041c3-b226-5552-6f56-7b081f4d8279") },
                    { new Guid("16e946f7-d088-7b8b-d375-327268e65ea8"), "Urban Greening and Beautification Projects — Floriculture Grade 12", "Urban Greening and Beautification Projects", 8, new Guid("f1ddfb49-3ee7-d0d0-3bdb-ba71e5d13aec") },
                    { new Guid("17bab82d-baff-f5aa-d979-2c2c5fd6750c"), "Introduction to the Internet and Email — Computer Studies Grade 10", "Introduction to the Internet and Email", 7, new Guid("43daec58-ceb4-fc4f-f1d4-086bd79d72a8") },
                    { new Guid("18425af0-bd0b-8af4-4062-d187a96facc4"), "Aquaculture Systems (Ponds, Cages, Raceways) — Fisheries Grade 11", "Aquaculture Systems (Ponds, Cages, Raceways)", 1, new Guid("77d36d32-edfb-63a5-b573-c9b18f58bb17") },
                    { new Guid("184f1291-f750-abfa-44c7-585fe1d7492c"), "Oral English (WASSCE Listening Test Prep) — English Language Grade 12", "Oral English (WASSCE Listening Test Prep)", 7, new Guid("9e7459ec-d351-be24-197d-c29757abab72") },
                    { new Guid("195086fb-8882-c5a7-f88b-4e4a7812c59d"), "Public Administration and Civil Service — Government Grade 11", "Public Administration and Civil Service", 2, new Guid("c5ad46a4-1f10-7aab-6b9d-9d32bca37e63") },
                    { new Guid("1a4387b4-01a3-9f06-9b3c-c59254654455"), "Summary Writing (Advanced Techniques) — English Language Grade 11", "Summary Writing (Advanced Techniques)", 2, new Guid("f081fc93-cdb6-c836-9743-149f52228aab") },
                    { new Guid("1a5fca97-770a-a0b6-63df-ede4d9e504db"), "Accounts of Non-Profit Organisations — Financial Accounting Grade 11", "Accounts of Non-Profit Organisations", 4, new Guid("c33eef6f-8e20-9183-f5a2-4e5187de42d7") },
                    { new Guid("1b487f6f-f452-a92a-a090-e0348675faf6"), "Classification of Horticultural Crops — Horticulture Grade 10", "Classification of Horticultural Crops", 2, new Guid("3c22945c-cc6c-6199-f614-746cb565217c") },
                    { new Guid("1bfe485e-ba00-4225-5862-2d6deecf6eba"), "Federalism and Unitarism — Government Grade 11", "Federalism and Unitarism", 5, new Guid("c5ad46a4-1f10-7aab-6b9d-9d32bca37e63") },
                    { new Guid("1c560b31-6e1f-ee40-3ae9-5e2c40dd0baf"), "Database Design and SQL Basics — Computer Studies Grade 12", "Database Design and SQL Basics", 3, new Guid("30f1307e-bbbc-d420-61c7-124673a9fd1c") },
                    { new Guid("1ca11756-83c2-4cc9-d653-60e0859bf279"), "Computer Networks (LAN, WAN, Internet Protocols) — Computer Studies Grade 11", "Computer Networks (LAN, WAN, Internet Protocols)", 6, new Guid("fffdd2fc-d6d3-7308-9d25-267ff832fd7f") },
                    { new Guid("1cedb1e4-eaea-bf02-5a36-95ee9aa36829"), "WASSCE Past Questions and Exam Strategies — English Language Grade 12", "WASSCE Past Questions and Exam Strategies", 10, new Guid("9e7459ec-d351-be24-197d-c29757abab72") },
                    { new Guid("1e1b925f-9c6b-c950-a5df-c098e2c84407"), "Information and Communication Technology in Business — Commerce Grade 11", "Information and Communication Technology in Business", 10, new Guid("773db100-257e-8d22-3695-0e647cbb7fd4") },
                    { new Guid("1eb2117f-2599-78cf-ebec-25cf8f46df3e"), "Boolean Algebra and Logic Gates — Computer Studies Grade 11", "Boolean Algebra and Logic Gates", 7, new Guid("fffdd2fc-d6d3-7308-9d25-267ff832fd7f") },
                    { new Guid("1eba05ad-34f5-d2e6-9f34-901d78c1d23a"), "Conic Sections (Circle, Parabola, Ellipse, Hyperbola) — Further Mathematics Grade 11", "Conic Sections (Circle, Parabola, Ellipse, Hyperbola)", 6, new Guid("a115558a-224b-bd96-8793-629375a2869e") },
                    { new Guid("1ec5aed5-874f-1f3a-0877-846256c15e7f"), "The Trans-Saharan Trade — History Grade 10", "The Trans-Saharan Trade", 4, new Guid("18c23abd-93c6-0e94-492c-dfd6d0a2ce28") },
                    { new Guid("1f305f91-a2b2-8935-118e-279dbfd7f5cd"), "Banking (Types of Banks, Services) — Commerce Grade 10", "Banking (Types of Banks, Services)", 6, new Guid("0ba6c666-e140-2220-8ec4-4f16bb2cd61b") },
                    { new Guid("1fe44886-9d1a-7d79-cb86-938745207ab1"), "Number Systems (Binary, Octal, Hexadecimal) — Computer Studies Grade 10", "Number Systems (Binary, Octal, Hexadecimal)", 4, new Guid("43daec58-ceb4-fc4f-f1d4-086bd79d72a8") },
                    { new Guid("200136d9-0176-7a2b-785d-45b4410764e8"), "Fish Quality Control and Food Safety — Fisheries Grade 12", "Fish Quality Control and Food Safety", 4, new Guid("aea3fc29-a3f7-76c0-8be7-f73335d78889") },
                    { new Guid("2044ce54-dae4-bf00-4652-f39a3721ab59"), "Desktop Publishing — Computer Studies Grade 11", "Desktop Publishing", 9, new Guid("fffdd2fc-d6d3-7308-9d25-267ff832fd7f") },
                    { new Guid("219ada12-2796-6ffd-116f-63c51722804a"), "The Life of Prophet Muhammad (Madinan Period) — Islamic Religious Studies Grade 11", "The Life of Prophet Muhammad (Madinan Period)", 1, new Guid("029f27a3-acc3-f8ab-b362-4a0d9c1db6f6") },
                    { new Guid("21e47548-4432-8a92-b421-0840079acb1f"), "Conflict Resolution and Peace — Christian Religious Studies Grade 12", "Conflict Resolution and Peace", 6, new Guid("30f59780-4140-3f0a-cded-8b13fd7052cb") },
                    { new Guid("224f067d-4317-ec91-4f93-e8020257fee7"), "Programming (Loops, Arrays, Functions) — Computer Studies Grade 12", "Programming (Loops, Arrays, Functions)", 2, new Guid("30f1307e-bbbc-d420-61c7-124673a9fd1c") },
                    { new Guid("227ff10a-f344-aa53-d75d-04a5421a4e7f"), "Demand, Supply and Price Determination — Economics Grade 10", "Demand, Supply and Price Determination", 3, new Guid("94f51606-1b58-9322-ecb2-cc8be90adce0") },
                    { new Guid("23c3f5cd-9642-361e-a165-060d54c7c90e"), "Fish Farming (Species Selection, Stocking) — Fisheries Grade 11", "Fish Farming (Species Selection, Stocking)", 2, new Guid("77d36d32-edfb-63a5-b573-c9b18f58bb17") },
                    { new Guid("25f5af40-8199-8138-1ef2-e787ef46d6c1"), "Balance of Trade and Balance of Payments — Commerce Grade 12", "Balance of Trade and Balance of Payments", 2, new Guid("6e0a7776-4921-0851-f05d-2a9cfe334aad") },
                    { new Guid("2682c933-8e04-9eb5-d750-a41580761767"), "Numerical Methods (Newton-Raphson, Trapezium Rule) — Further Mathematics Grade 12", "Numerical Methods (Newton-Raphson, Trapezium Rule)", 5, new Guid("c0eae66d-d3fd-8079-f377-be72f93a2d06") },
                    { new Guid("268a9f60-6048-c9b7-af3d-5f75d2ea6d2f"), "Home Trade (Wholesale and Retail) — Commerce Grade 10", "Home Trade (Wholesale and Retail)", 2, new Guid("0ba6c666-e140-2220-8ec4-4f16bb2cd61b") },
                    { new Guid("290c1b1b-0c7f-b400-d727-54f2f28f677d"), "Flower Anatomy and Physiology — Floriculture Grade 10", "Flower Anatomy and Physiology", 3, new Guid("4b421fd5-92e6-ef02-edd7-7804412bedd0") },
                    { new Guid("2a0f3bfc-6242-5f79-b948-6e4b8f0688c2"), "Forms of Government (Democracy, Monarchy, Oligarchy) — Government Grade 10", "Forms of Government (Democracy, Monarchy, Oligarchy)", 2, new Guid("c0e3b77a-b7e6-f84c-1414-a9b4986364c7") },
                    { new Guid("2af0b667-4dae-b4e3-07d6-e35d7d8601b6"), "Tenses and Concord — English Language Grade 10", "Tenses and Concord", 5, new Guid("def2214a-a301-249e-c656-0914b4ecb5a5") },
                    { new Guid("2b1fe552-f577-f839-8810-6da3d5960204"), "The United Nations and Africa — History Grade 12", "The United Nations and Africa", 5, new Guid("8c240df7-cbb8-b63b-c2fa-15ef2ff299d6") },
                    { new Guid("2b6a59c7-c059-e196-201d-683fb17834f0"), "Post-Harvest Handling and Storage — Horticulture Grade 11", "Post-Harvest Handling and Storage", 9, new Guid("efa84916-91b0-0dea-73a8-860fc9a80941") },
                    { new Guid("2b74fcc5-96ee-3d64-a903-2ae430f39447"), "Tafsir (Quranic Exegesis) of Selected Passages — Islamic Religious Studies Grade 12", "Tafsir (Quranic Exegesis) of Selected Passages", 6, new Guid("0a0e509f-a99c-3193-c277-928d54224cc2") },
                    { new Guid("2c6f9b0a-9389-4254-1b38-486bd405553b"), "WASSCE Past Questions Practice — Computer Studies Grade 12", "WASSCE Past Questions Practice", 10, new Guid("30f1307e-bbbc-d420-61c7-124673a9fd1c") },
                    { new Guid("2cb4b257-5efc-287e-c8ff-a697137bca70"), "Trigonometric Functions and Identities — Further Mathematics Grade 10", "Trigonometric Functions and Identities", 8, new Guid("a4eaeeb3-b7a3-9abd-1c53-5309c8d473c0") },
                    { new Guid("2ccef70f-8543-4d6a-d6c1-f7d0b1620775"), "Landscape Design and Installation — Floriculture Grade 11", "Landscape Design and Installation", 5, new Guid("49841f61-c212-8eec-2810-f1bb824e37d2") },
                    { new Guid("2d147903-ad40-92f7-a297-e02349fa6f8c"), "Manufacturing Accounts — Financial Accounting Grade 11", "Manufacturing Accounts", 5, new Guid("c33eef6f-8e20-9183-f5a2-4e5187de42d7") },
                    { new Guid("2dd67d8f-4a28-daa2-03a0-a073f677dc38"), "Formal Letter Writing and Reports — English Language Grade 11", "Formal Letter Writing and Reports", 6, new Guid("f081fc93-cdb6-c836-9743-149f52228aab") },
                    { new Guid("2e742cb0-f9b9-39d6-a22d-aeb702353fb8"), "Vectors (Two and Three Dimensions) — Further Mathematics Grade 10", "Vectors (Two and Three Dimensions)", 10, new Guid("a4eaeeb3-b7a3-9abd-1c53-5309c8d473c0") },
                    { new Guid("3129601a-0b01-f9a2-9e17-42cfb26cb39a"), "Potted Plants and Indoor Gardening — Floriculture Grade 10", "Potted Plants and Indoor Gardening", 8, new Guid("4b421fd5-92e6-ef02-edd7-7804412bedd0") },
                    { new Guid("31dd619a-033f-5f35-ded2-0a82e653b8ac"), "Fertiliser Use in Horticulture — Horticulture Grade 11", "Fertiliser Use in Horticulture", 7, new Guid("efa84916-91b0-0dea-73a8-860fc9a80941") },
                    { new Guid("332fa2ac-cc6b-7a57-2e6b-6f7dea6ac3da"), "Business Ethics and Social Responsibility — Commerce Grade 12", "Business Ethics and Social Responsibility", 5, new Guid("6e0a7776-4921-0851-f05d-2a9cfe334aad") },
                    { new Guid("33e7b244-5ec8-e69b-2add-9bbe17229cec"), "Foliage and Tropical Ornamental Plants — Floriculture Grade 12", "Foliage and Tropical Ornamental Plants", 3, new Guid("f1ddfb49-3ee7-d0d0-3bdb-ba71e5d13aec") },
                    { new Guid("345a783b-fab9-35bf-3a41-51ccdd345987"), "International Fisheries Organisations — Fisheries Grade 12", "International Fisheries Organisations", 7, new Guid("aea3fc29-a3f7-76c0-8be7-f73335d78889") },
                    { new Guid("34e41a03-6676-f545-ddac-557c3464eeee"), "Climate and Soil Requirements for Horticulture — Horticulture Grade 10", "Climate and Soil Requirements for Horticulture", 3, new Guid("3c22945c-cc6c-6199-f614-746cb565217c") },
                    { new Guid("34fa58ba-38ed-8754-54af-23a9b536ef01"), "The Birth and Early Life of Jesus — Christian Religious Studies Grade 10", "The Birth and Early Life of Jesus", 10, new Guid("35b0b395-4f39-535e-d197-cb82f839fbbc") },
                    { new Guid("3521dda1-4206-1fa4-42b1-74bfb465bee9"), "WASSCE Past Questions Practice — Financial Accounting Grade 12", "WASSCE Past Questions Practice", 10, new Guid("4d512f11-4373-8e2f-543c-0842a91f4cf2") },
                    { new Guid("35b2d6a2-9f9a-5c4b-9524-d4bb4011026d"), "Common Errors in English Usage — English Language Grade 11", "Common Errors in English Usage", 10, new Guid("f081fc93-cdb6-c836-9743-149f52228aab") },
                    { new Guid("35e86414-0fc9-3ef6-1131-fcf18b25cd63"), "Vegetable Production (Common Vegetables) — Horticulture Grade 10", "Vegetable Production (Common Vegetables)", 6, new Guid("3c22945c-cc6c-6199-f614-746cb565217c") },
                    { new Guid("366db323-4ac1-9fac-892e-9037819bb038"), "Petroleum and the Nigerian Economy — Economics Grade 12", "Petroleum and the Nigerian Economy", 4, new Guid("0d331ad8-0343-b862-4378-b5f017817a63") },
                    { new Guid("372aa319-6e6f-2765-fbe6-0c66e770b2b3"), "Linear Transformations — Further Mathematics Grade 11", "Linear Transformations", 3, new Guid("a115558a-224b-bd96-8793-629375a2869e") },
                    { new Guid("3770dbf0-a610-2604-6b6f-2907e7001df3"), "Fish Anatomy and Physiology — Fisheries Grade 10", "Fish Anatomy and Physiology", 4, new Guid("70a66f3c-d5f9-1b35-bc9c-119ef3d85f3e") },
                    { new Guid("385bd0f0-1235-33df-79b6-4a41b90499d6"), "Grammar Revision (Exam Focus) — English Language Grade 12", "Grammar Revision (Exam Focus)", 8, new Guid("9e7459ec-d351-be24-197d-c29757abab72") },
                    { new Guid("39563711-23a9-f93f-17c0-06be2a82426b"), "Government of Ghana (Comparison) — Government Grade 12", "Government of Ghana (Comparison)", 2, new Guid("661ec45c-8411-3167-8af8-544152cebce6") },
                    { new Guid("39e6eb24-3b59-861a-bdd6-1f82086d2292"), "Agriculture and Industry in West Africa — Economics Grade 11", "Agriculture and Industry in West Africa", 7, new Guid("04fbf1c3-6b48-d6c9-e60b-8af777b7fa50") },
                    { new Guid("3b3a9afd-2f77-34c9-4e78-8cf36fd336d7"), "Elections and Electoral Systems — Government Grade 10", "Elections and Electoral Systems", 10, new Guid("c0e3b77a-b7e6-f84c-1414-a9b4986364c7") },
                    { new Guid("3c0a8007-b68a-1dab-b222-b48f657abd7e"), "Export and Import of Flowers — Floriculture Grade 12", "Export and Import of Flowers", 2, new Guid("f1ddfb49-3ee7-d0d0-3bdb-ba71e5d13aec") },
                    { new Guid("3d98392f-cfb0-8774-01af-4de9eaa817d1"), "Summary Writing (WASSCE Paper 2 Practice) — English Language Grade 12", "Summary Writing (WASSCE Paper 2 Practice)", 2, new Guid("9e7459ec-d351-be24-197d-c29757abab72") },
                    { new Guid("3f0cc97e-578d-a456-01c2-b952d42fad27"), "Speech Writing and Debate — English Language Grade 11", "Speech Writing and Debate", 8, new Guid("f081fc93-cdb6-c836-9743-149f52228aab") },
                    { new Guid("414a97cd-1ced-8249-1cf8-c014ea1173c4"), "Environmental Impact of Fishing and Aquaculture — Fisheries Grade 12", "Environmental Impact of Fishing and Aquaculture", 5, new Guid("aea3fc29-a3f7-76c0-8be7-f73335d78889") },
                    { new Guid("424c681b-32f0-880b-d53d-c510d38c34b7"), "The Sermon on the Mount — Christian Religious Studies Grade 11", "The Sermon on the Mount", 3, new Guid("97ec5abf-b57f-0843-fe64-8f31af424c93") },
                    { new Guid("42e0298d-d193-bf8c-1006-5a401316ef44"), "Types of Fishing (Artisanal, Industrial, Recreational) — Fisheries Grade 10", "Types of Fishing (Artisanal, Industrial, Recreational)", 6, new Guid("70a66f3c-d5f9-1b35-bc9c-119ef3d85f3e") },
                    { new Guid("439c4bb9-3f75-22b8-20c3-f865d740ad79"), "Factors of Production — Economics Grade 10", "Factors of Production", 6, new Guid("94f51606-1b58-9322-ecb2-cc8be90adce0") },
                    { new Guid("43fec339-3100-7e54-870d-f8ec34bc0094"), "Moral Teachings in Islam — Islamic Religious Studies Grade 11", "Moral Teachings in Islam", 10, new Guid("029f27a3-acc3-f8ab-b362-4a0d9c1db6f6") },
                    { new Guid("44b57ca8-bbe7-05db-6e76-f4a23cc572c3"), "Plant Growth Regulators in Floriculture — Floriculture Grade 11", "Plant Growth Regulators in Floriculture", 3, new Guid("49841f61-c212-8eec-2810-f1bb824e37d2") },
                    { new Guid("45cda1ef-c11c-ed3c-8251-189aa7b0896d"), "Christian Ethics (Love, Justice, Forgiveness) — Christian Religious Studies Grade 11", "Christian Ethics (Love, Justice, Forgiveness)", 8, new Guid("97ec5abf-b57f-0843-fe64-8f31af424c93") },
                    { new Guid("460efc92-a33f-64d7-ee2e-d3a8f8b5110d"), "Plant Growth Regulators and Hormones — Horticulture Grade 11", "Plant Growth Regulators and Hormones", 3, new Guid("efa84916-91b0-0dea-73a8-860fc9a80941") },
                    { new Guid("46201024-bbf9-fa41-4343-5310f6be18b7"), "Flower Shows and Exhibitions — Floriculture Grade 11", "Flower Shows and Exhibitions", 10, new Guid("49841f61-c212-8eec-2810-f1bb824e37d2") },
                    { new Guid("463f2576-8c43-df01-cb9e-30b3cd8c57ff"), "Presentation Software (Microsoft PowerPoint) — Computer Studies Grade 11", "Presentation Software (Microsoft PowerPoint)", 3, new Guid("fffdd2fc-d6d3-7308-9d25-267ff832fd7f") },
                    { new Guid("464f00e0-3028-f0d3-9e7a-2e76fa02f610"), "Government and Business Regulation — Commerce Grade 11", "Government and Business Regulation", 9, new Guid("773db100-257e-8d22-3695-0e647cbb7fd4") },
                    { new Guid("464fa3f4-7212-18d4-1829-66e8bd3438a0"), "Ornamental Plants and Landscaping — Horticulture Grade 11", "Ornamental Plants and Landscaping", 1, new Guid("efa84916-91b0-0dea-73a8-860fc9a80941") },
                    { new Guid("4668fa90-9584-0cf3-3032-9fa4b6096dce"), "Polynomials (Factor and Remainder Theorem) — Further Mathematics Grade 10", "Polynomials (Factor and Remainder Theorem)", 3, new Guid("a4eaeeb3-b7a3-9abd-1c53-5309c8d473c0") },
                    { new Guid("46c8a933-1e73-70be-1747-cfb495060260"), "Economic Systems (Market, Command, Mixed) — Economics Grade 10", "Economic Systems (Market, Command, Mixed)", 2, new Guid("94f51606-1b58-9322-ecb2-cc8be90adce0") },
                    { new Guid("47544b06-cc4c-846d-7dfc-1f4cfdc5dbbe"), "Political Culture and Socialisation — Government Grade 11", "Political Culture and Socialisation", 3, new Guid("c5ad46a4-1f10-7aab-6b9d-9d32bca37e63") },
                    { new Guid("479df6a9-ef62-6ca0-4942-216cecd8fb0c"), "Political Participation and Citizenship — Government Grade 10", "Political Participation and Citizenship", 5, new Guid("c0e3b77a-b7e6-f84c-1414-a9b4986364c7") },
                    { new Guid("47a7ce5b-89e8-1026-7943-0b6b7ba94a74"), "Trial Balance — Financial Accounting Grade 10", "Trial Balance", 6, new Guid("cd368ced-9b4b-53c2-f818-261143f97c4b") },
                    { new Guid("481881b7-ff10-cee9-317e-adf55d0e1a64"), "Moses and the Exodus — Christian Religious Studies Grade 10", "Moses and the Exodus", 3, new Guid("35b0b395-4f39-535e-d197-cb82f839fbbc") },
                    { new Guid("483742e5-d73e-1ab4-f6ac-eb8ffa2df637"), "Data and Information — Computer Studies Grade 10", "Data and Information", 9, new Guid("43daec58-ceb4-fc4f-f1d4-086bd79d72a8") },
                    { new Guid("4870d615-a236-4ba1-0341-3aa4e6c97310"), "Garden Design Principles — Floriculture Grade 10", "Garden Design Principles", 9, new Guid("4b421fd5-92e6-ef02-edd7-7804412bedd0") },
                    { new Guid("4891705d-e248-9e45-d9dc-e21ae231afb8"), "Lexis and Structure (Vocabulary Building) — English Language Grade 10", "Lexis and Structure (Vocabulary Building)", 3, new Guid("def2214a-a301-249e-c656-0914b4ecb5a5") },
                    { new Guid("49161299-aadb-c6ef-7d77-bf187211977b"), "European Exploration and Contact — History Grade 10", "European Exploration and Contact", 7, new Guid("18c23abd-93c6-0e94-492c-dfd6d0a2ce28") },
                    { new Guid("49ec01d9-fe64-5715-76ea-97e1f52fd8c1"), "Weed Management in Horticultural Crops — Horticulture Grade 11", "Weed Management in Horticultural Crops", 8, new Guid("efa84916-91b0-0dea-73a8-860fc9a80941") },
                    { new Guid("4a6c6d9b-09bb-b459-2c16-87ac13af9061"), "Fisheries and Climate Change — Fisheries Grade 12", "Fisheries and Climate Change", 6, new Guid("aea3fc29-a3f7-76c0-8be7-f73335d78889") },
                    { new Guid("4b20b1d8-e819-c56a-2db3-a6403775fcae"), "Royalty Accounts — Financial Accounting Grade 12", "Royalty Accounts", 7, new Guid("4d512f11-4373-8e2f-543c-0842a91f4cf2") },
                    { new Guid("4c658e9f-10ee-923a-9528-9c3c7809aca6"), "Advanced Propagation Techniques (Tissue Culture) — Horticulture Grade 12", "Advanced Propagation Techniques (Tissue Culture)", 1, new Guid("e868e7f9-be26-b7fe-6bff-f3ca7fd9b926") },
                    { new Guid("4ceb0762-621f-e1f6-e82d-717901ee7776"), "Agribusiness in Horticulture — Horticulture Grade 12", "Agribusiness in Horticulture", 8, new Guid("e868e7f9-be26-b7fe-6bff-f3ca7fd9b926") },
                    { new Guid("4d6ce23b-9743-4d32-a88e-960d9221d3d6"), "The Ministry of Jesus (Teachings and Parables) — Christian Religious Studies Grade 11", "The Ministry of Jesus (Teachings and Parables)", 1, new Guid("97ec5abf-b57f-0843-fe64-8f31af424c93") },
                    { new Guid("4de7d120-a521-a277-4c45-6cc6c9cb9a8e"), "Contemporary Political Issues — Government Grade 12", "Contemporary Political Issues", 9, new Guid("661ec45c-8411-3167-8af8-544152cebce6") },
                    { new Guid("4e488e6e-a6ca-a411-ce6a-1e33b6986d16"), "Tools and Equipment for Floriculture — Floriculture Grade 10", "Tools and Equipment for Floriculture", 10, new Guid("4b421fd5-92e6-ef02-edd7-7804412bedd0") },
                    { new Guid("51994855-d2f8-098c-efd3-5da7fee70cdc"), "Aquatic Ecosystems (Freshwater, Marine, Brackish) — Fisheries Grade 10", "Aquatic Ecosystems (Freshwater, Marine, Brackish)", 2, new Guid("70a66f3c-d5f9-1b35-bc9c-119ef3d85f3e") },
                    { new Guid("51abd258-81a5-c246-db02-c075434fbe4b"), "Government of Nigeria (Comparison) — Government Grade 12", "Government of Nigeria (Comparison)", 1, new Guid("661ec45c-8411-3167-8af8-544152cebce6") },
                    { new Guid("5423176d-4cf0-689f-4bff-f8dd708c8cc1"), "Pre-Colonial Societies in West Africa — History Grade 10", "Pre-Colonial Societies in West Africa", 3, new Guid("18c23abd-93c6-0e94-492c-dfd6d0a2ce28") },
                    { new Guid("545e7a46-de5c-9410-03c4-f9016ec4b911"), "Tissue Culture in Floriculture — Floriculture Grade 12", "Tissue Culture in Floriculture", 5, new Guid("f1ddfb49-3ee7-d0d0-3bdb-ba71e5d13aec") },
                    { new Guid("5522b60e-5ae9-35bd-d1b9-6d12e8565de1"), "Lexis and Structure (Idioms, Phrasal Verbs) — English Language Grade 11", "Lexis and Structure (Idioms, Phrasal Verbs)", 3, new Guid("f081fc93-cdb6-c836-9743-149f52228aab") },
                    { new Guid("559cbb9a-8a85-d896-3f43-d2124539a2f2"), "Lexis and Structure (Exam-Level Vocabulary) — English Language Grade 12", "Lexis and Structure (Exam-Level Vocabulary)", 3, new Guid("9e7459ec-d351-be24-197d-c29757abab72") },
                    { new Guid("581db8be-6237-041a-ebe4-f8022928cb2d"), "The Life of Prophet Muhammad (Makkan Period) — Islamic Religious Studies Grade 10", "The Life of Prophet Muhammad (Makkan Period)", 6, new Guid("598041c3-b226-5552-6f56-7b081f4d8279") },
                    { new Guid("58f2e1f2-11a8-a9fd-f23c-a85662e20f7c"), "Arms of Government (Legislature, Executive, Judiciary) — Government Grade 10", "Arms of Government (Legislature, Executive, Judiciary)", 3, new Guid("c0e3b77a-b7e6-f84c-1414-a9b4986364c7") },
                    { new Guid("59370825-361b-f3bb-3c5f-9a7fbb95b33f"), "Departmental Accounts — Financial Accounting Grade 11", "Departmental Accounts", 10, new Guid("c33eef6f-8e20-9183-f5a2-4e5187de42d7") },
                    { new Guid("59c82afd-e635-b5a0-a637-45a7a300e6e4"), "Mechanics (Kinematics) — Further Mathematics Grade 11", "Mechanics (Kinematics)", 10, new Guid("a115558a-224b-bd96-8793-629375a2869e") },
                    { new Guid("59f2429b-0cff-f3fc-400d-8fa5db4ec908"), "E-Commerce and Digital Economy — Computer Studies Grade 12", "E-Commerce and Digital Economy", 6, new Guid("30f1307e-bbbc-d420-61c7-124673a9fd1c") },
                    { new Guid("59f496db-8cda-b0bb-479b-999a22c66b4a"), "Advertising and Sales Promotion — Commerce Grade 11", "Advertising and Sales Promotion", 3, new Guid("773db100-257e-8d22-3695-0e647cbb7fd4") },
                    { new Guid("5b0688b4-d23a-e306-f7e4-5ce33c55c07e"), "Comparative Religion (Islam, Christianity, ATR) — Islamic Religious Studies Grade 12", "Comparative Religion (Islam, Christianity, ATR)", 5, new Guid("0a0e509f-a99c-3193-c277-928d54224cc2") },
                    { new Guid("5b4fa2e4-a927-595a-d10b-e59c07993e41"), "Selected Psalms and Proverbs — Christian Religious Studies Grade 10", "Selected Psalms and Proverbs", 8, new Guid("35b0b395-4f39-535e-d197-cb82f839fbbc") },
                    { new Guid("5cb614e7-ea6a-1436-ac74-ad4915253d2a"), "Mariculture (Shrimp, Oyster, Seaweed Farming) — Fisheries Grade 12", "Mariculture (Shrimp, Oyster, Seaweed Farming)", 2, new Guid("aea3fc29-a3f7-76c0-8be7-f73335d78889") },
                    { new Guid("5dcad4b1-e035-e6b1-20b3-d5d5e126cfff"), "The Scramble for and Partition of Africa — History Grade 10", "The Scramble for and Partition of Africa", 8, new Guid("18c23abd-93c6-0e94-492c-dfd6d0a2ce28") },
                    { new Guid("5e26de40-0c9e-15c8-106f-72ba66d74dc8"), "Pest and Disease Management in Flowers — Floriculture Grade 11", "Pest and Disease Management in Flowers", 7, new Guid("49841f61-c212-8eec-2810-f1bb824e37d2") },
                    { new Guid("5e2b35b2-46d4-6907-b8af-ed06ada31e20"), "Post-Independence Challenges — History Grade 11", "Post-Independence Challenges", 9, new Guid("a33a76d9-ae1b-fb53-fef6-83cfc12992bb") },
                    { new Guid("5f8383bc-e3bd-0f9e-cf47-dc1402920124"), "Tawhid (Oneness of Allah) — Islamic Religious Studies Grade 10", "Tawhid (Oneness of Allah)", 1, new Guid("598041c3-b226-5552-6f56-7b081f4d8279") },
                    { new Guid("5f89fd25-22fa-cc32-0f2c-cf4a2721e385"), "Market Structures (Perfect Competition, Monopoly, Oligopoly) — Economics Grade 11", "Market Structures (Perfect Competition, Monopoly, Oligopoly)", 1, new Guid("04fbf1c3-6b48-d6c9-e60b-8af777b7fa50") },
                    { new Guid("5fba14d0-46b3-144e-8c42-bd68bba1685b"), "Nursery Management for Flowers — Floriculture Grade 10", "Nursery Management for Flowers", 6, new Guid("4b421fd5-92e6-ef02-edd7-7804412bedd0") },
                    { new Guid("5fc4e27b-dbe0-39fc-d9df-634686f2fa0e"), "Islamic Economic System — Islamic Religious Studies Grade 11", "Islamic Economic System", 6, new Guid("029f27a3-acc3-f8ab-b362-4a0d9c1db6f6") },
                    { new Guid("5fcb450c-7553-d7ce-e96d-35cc0c6986fa"), "Drying and Preservation of Flowers — Floriculture Grade 11", "Drying and Preservation of Flowers", 9, new Guid("49841f61-c212-8eec-2810-f1bb824e37d2") },
                    { new Guid("60715326-cfc0-637d-9ae7-1c8dfe0f00c0"), "The Missionary Journeys of Paul — Christian Religious Studies Grade 11", "The Missionary Journeys of Paul", 6, new Guid("97ec5abf-b57f-0843-fe64-8f31af424c93") },
                    { new Guid("618ce8ef-36b1-b7bc-3100-7a0eaaaefdc6"), "Business Organisations (Sole Trader, Partnerships, Companies) — Economics Grade 10", "Business Organisations (Sole Trader, Partnerships, Companies)", 7, new Guid("94f51606-1b58-9322-ecb2-cc8be90adce0") },
                    { new Guid("62639995-6184-826c-bdbf-8280a73efe59"), "Marriage and Family Life — Christian Religious Studies Grade 11", "Marriage and Family Life", 9, new Guid("97ec5abf-b57f-0843-fe64-8f31af424c93") },
                    { new Guid("6294c248-a183-2fa7-2ef4-0aeed9f9a3a7"), "The Ten Commandments and the Covenant — Christian Religious Studies Grade 10", "The Ten Commandments and the Covenant", 4, new Guid("35b0b395-4f39-535e-d197-cb82f839fbbc") },
                    { new Guid("62dc0061-a6f6-dcae-8c5d-c4fd34179834"), "Creation and the Fall of Man — Christian Religious Studies Grade 10", "Creation and the Fall of Man", 1, new Guid("35b0b395-4f39-535e-d197-cb82f839fbbc") },
                    { new Guid("64499a27-509c-af71-2494-c63c86626a07"), "Statistics (Normal Distribution, Hypothesis Testing) — Further Mathematics Grade 12", "Statistics (Normal Distribution, Hypothesis Testing)", 7, new Guid("c0eae66d-d3fd-8079-f377-be72f93a2d06") },
                    { new Guid("65287e2c-455f-ea29-7cf0-3add99ecc655"), "Fisheries Legislation and Regulations — Fisheries Grade 11", "Fisheries Legislation and Regulations", 8, new Guid("77d36d32-edfb-63a5-b573-c9b18f58bb17") },
                    { new Guid("6570128f-2119-c14f-7b5b-80db9bb48d52"), "Letter Writing (Formal, Semi-Formal, Informal) — English Language Grade 12", "Letter Writing (Formal, Semi-Formal, Informal)", 5, new Guid("9e7459ec-d351-be24-197d-c29757abab72") },
                    { new Guid("65761251-76e4-78ef-3af9-335cb5830426"), "The Rightly Guided Caliphs (Abu Bakr, Umar, Uthman, Ali) — Islamic Religious Studies Grade 11", "The Rightly Guided Caliphs (Abu Bakr, Umar, Uthman, Ali)", 2, new Guid("029f27a3-acc3-f8ab-b362-4a0d9c1db6f6") },
                    { new Guid("6635fedf-60e5-331f-ea6d-72bcfde958a5"), "Communication in Business — Commerce Grade 10", "Communication in Business", 9, new Guid("0ba6c666-e140-2220-8ec4-4f16bb2cd61b") },
                    { new Guid("67124016-1d49-4d91-ed31-dcf88116f421"), "Fundamental Human Rights — Government Grade 10", "Fundamental Human Rights", 7, new Guid("c0e3b77a-b7e6-f84c-1414-a9b4986364c7") },
                    { new Guid("68305741-a8de-c198-1e14-77bef1cdb44d"), "Public Finance (Government Revenue and Expenditure) — Economics Grade 11", "Public Finance (Government Revenue and Expenditure)", 6, new Guid("04fbf1c3-6b48-d6c9-e60b-8af777b7fa50") },
                    { new Guid("6aaa7896-d28d-6cb4-9bca-59c9429adad5"), "Islamic Movements in West Africa (Jihads) — Islamic Religious Studies Grade 12", "Islamic Movements in West Africa (Jihads)", 2, new Guid("0a0e509f-a99c-3193-c277-928d54224cc2") },
                    { new Guid("6b13a098-94be-156a-f1d6-72bb14847c8d"), "Programming in BASIC or Python (Fundamentals) — Computer Studies Grade 11", "Programming in BASIC or Python (Fundamentals)", 5, new Guid("fffdd2fc-d6d3-7308-9d25-267ff832fd7f") },
                    { new Guid("6cc29ab5-b2a6-39fb-17a7-a502182ae878"), "Nursery Establishment and Management — Horticulture Grade 10", "Nursery Establishment and Management", 5, new Guid("3c22945c-cc6c-6199-f614-746cb565217c") },
                    { new Guid("6d3dccc9-dad4-46e4-874e-ed9edb68c8b4"), "Computer Software (System and Application Software) — Computer Studies Grade 10", "Computer Software (System and Application Software)", 3, new Guid("43daec58-ceb4-fc4f-f1d4-086bd79d72a8") },
                    { new Guid("6d79663c-eef9-f8de-c939-1cd1b5430436"), "Cybersecurity and Data Protection — Computer Studies Grade 12", "Cybersecurity and Data Protection", 5, new Guid("30f1307e-bbbc-d420-61c7-124673a9fd1c") },
                    { new Guid("6e8bfc6c-36de-e700-9c5d-0f54ac2898ab"), "Business Organisations (Types and Formation) — Commerce Grade 10", "Business Organisations (Types and Formation)", 10, new Guid("0ba6c666-e140-2220-8ec4-4f16bb2cd61b") },
                    { new Guid("6ea4d355-143a-bbcc-8cca-988a367b8408"), "Integration (Techniques, Definite Integrals) — Further Mathematics Grade 11", "Integration (Techniques, Definite Integrals)", 5, new Guid("a115558a-224b-bd96-8793-629375a2869e") },
                    { new Guid("6ef85e32-2dc3-fb77-8ee9-5f20841e8dfd"), "Resistance to Colonial Rule — History Grade 11", "Resistance to Colonial Rule", 3, new Guid("a33a76d9-ae1b-fb53-fef6-83cfc12992bb") },
                    { new Guid("6ffa6dc9-7365-1355-df29-ca8ba4304116"), "Colonial Economy and Social Change — History Grade 11", "Colonial Economy and Social Change", 2, new Guid("a33a76d9-ae1b-fb53-fef6-83cfc12992bb") },
                    { new Guid("7086bc64-a522-d370-3a01-70d9d1332870"), "Hadith (Introduction, Classification) — Islamic Religious Studies Grade 10", "Hadith (Introduction, Classification)", 7, new Guid("598041c3-b226-5552-6f56-7b081f4d8279") },
                    { new Guid("70c74db6-d260-d61e-5e0d-d54473f56337"), "Leadership and Service in the Church — Christian Religious Studies Grade 12", "Leadership and Service in the Church", 7, new Guid("30f59780-4140-3f0a-cded-8b13fd7052cb") },
                    { new Guid("7144fade-f89d-3858-3647-7488cd5ff5dc"), "Nationalism and Independence Movements — Government Grade 11", "Nationalism and Independence Movements", 8, new Guid("c5ad46a4-1f10-7aab-6b9d-9d32bca37e63") },
                    { new Guid("71f45318-fdf1-7ef6-b431-8cf4c229153f"), "The Military in Politics — Government Grade 11", "The Military in Politics", 4, new Guid("c5ad46a4-1f10-7aab-6b9d-9d32bca37e63") },
                    { new Guid("7262fa46-2516-9699-26af-95ca78c6a406"), "Human Rights and the Rule of Law in Practice — Government Grade 12", "Human Rights and the Rule of Law in Practice", 6, new Guid("661ec45c-8411-3167-8af8-544152cebce6") },
                    { new Guid("72aac255-9983-0af8-616d-b6bf9b3355bc"), "Introduction to Horticulture (Scope and Importance) — Horticulture Grade 10", "Introduction to Horticulture (Scope and Importance)", 1, new Guid("3c22945c-cc6c-6199-f614-746cb565217c") },
                    { new Guid("73858aee-88d1-b56d-3e4a-1463560bbb64"), "Dissolution of Partnership — Financial Accounting Grade 11", "Dissolution of Partnership", 8, new Guid("c33eef6f-8e20-9183-f5a2-4e5187de42d7") },
                    { new Guid("73b82679-102c-6f94-21c1-344f103c55e4"), "The Holy Spirit and Christian Living — Christian Religious Studies Grade 12", "The Holy Spirit and Christian Living", 4, new Guid("30f59780-4140-3f0a-cded-8b13fd7052cb") },
                    { new Guid("73ce0e90-3030-f5eb-96e0-b42fd27028bd"), "WASSCE Past Questions Practice — Horticulture Grade 12", "WASSCE Past Questions Practice", 10, new Guid("e868e7f9-be26-b7fe-6bff-f3ca7fd9b926") },
                    { new Guid("75077c02-416b-3a66-95e2-960f71df1cf5"), "The Stock Exchange and Capital Market — Commerce Grade 11", "The Stock Exchange and Capital Market", 1, new Guid("773db100-257e-8d22-3695-0e647cbb7fd4") },
                    { new Guid("759b08fa-3223-d872-2003-73daca8ee247"), "Entrepreneurship and Small Business Management — Commerce Grade 12", "Entrepreneurship and Small Business Management", 6, new Guid("6e0a7776-4921-0851-f05d-2a9cfe334aad") },
                    { new Guid("75b8b1da-49bb-a882-5e6f-a8bd0ceedb79"), "Horticultural Crop Improvement — Horticulture Grade 12", "Horticultural Crop Improvement", 6, new Guid("e868e7f9-be26-b7fe-6bff-f3ca7fd9b926") },
                    { new Guid("7605e15d-03ea-430f-e766-20487a1b9646"), "Basic Economic Concepts (Scarcity, Choice, Opportunity Cost) — Economics Grade 10", "Basic Economic Concepts (Scarcity, Choice, Opportunity Cost)", 1, new Guid("94f51606-1b58-9322-ecb2-cc8be90adce0") },
                    { new Guid("762de910-7db7-2920-5a30-9b0d2e4a7023"), "Branch Accounts — Financial Accounting Grade 12", "Branch Accounts", 5, new Guid("4d512f11-4373-8e2f-543c-0842a91f4cf2") },
                    { new Guid("7633dea0-d21b-2214-727b-c10e1d498b73"), "Islamic Influence in West Africa — History Grade 10", "Islamic Influence in West Africa", 6, new Guid("18c23abd-93c6-0e94-492c-dfd6d0a2ce28") },
                    { new Guid("769fa036-f03f-eaf2-fe00-7cf850bdbf4c"), "Classification of Fish and Aquatic Organisms — Fisheries Grade 10", "Classification of Fish and Aquatic Organisms", 3, new Guid("70a66f3c-d5f9-1b35-bc9c-119ef3d85f3e") },
                    { new Guid("77875edc-f504-eeea-afbf-bdea83ff28d6"), "Entrepreneurship in Fisheries — Fisheries Grade 12", "Entrepreneurship in Fisheries", 9, new Guid("aea3fc29-a3f7-76c0-8be7-f73335d78889") },
                    { new Guid("78b8fe0b-da9f-797b-f8ce-1983ad7204ac"), "Post-Independence Political Development in West Africa — History Grade 12", "Post-Independence Political Development in West Africa", 1, new Guid("8c240df7-cbb8-b63b-c2fa-15ef2ff299d6") },
                    { new Guid("79692199-9913-c00b-1422-24f79954003e"), "Computer Safety, Ethics and Security — Computer Studies Grade 10", "Computer Safety, Ethics and Security", 8, new Guid("43daec58-ceb4-fc4f-f1d4-086bd79d72a8") },
                    { new Guid("7a1ab915-e81a-6bf4-c42b-d0fbb53c3e8b"), "Islam and Contemporary Issues — Islamic Religious Studies Grade 12", "Islam and Contemporary Issues", 3, new Guid("0a0e509f-a99c-3193-c277-928d54224cc2") },
                    { new Guid("7a21a118-3382-fd30-e462-1dd6d404f58a"), "Colonial Administration (British, French, Portuguese) — History Grade 11", "Colonial Administration (British, French, Portuguese)", 1, new Guid("a33a76d9-ae1b-fb53-fef6-83cfc12992bb") },
                    { new Guid("7c59897f-3065-8224-9c30-90f3045cc12d"), "Ornamental Fish Keeping — Fisheries Grade 11", "Ornamental Fish Keeping", 9, new Guid("77d36d32-edfb-63a5-b573-c9b18f58bb17") },
                    { new Guid("7cc21cb8-e89a-9713-274a-7bed98bce15a"), "Irrigation and Water Management in Horticulture — Horticulture Grade 10", "Irrigation and Water Management in Horticulture", 9, new Guid("3c22945c-cc6c-6199-f614-746cb565217c") },
                    { new Guid("7cde5f2d-9c81-6479-be61-e33e495d7ace"), "Bank Reconciliation Statement — Financial Accounting Grade 10", "Bank Reconciliation Statement", 9, new Guid("cd368ced-9b4b-53c2-f818-261143f97c4b") },
                    { new Guid("7d0a65e0-92dd-e83d-1c2a-7cca95191241"), "Quadratic and Cubic Equations — Further Mathematics Grade 10", "Quadratic and Cubic Equations", 4, new Guid("a4eaeeb3-b7a3-9abd-1c53-5309c8d473c0") },
                    { new Guid("7e1d6e0b-a6fd-0585-daec-6fee40be7c27"), "Pre-Colonial Political Systems in West Africa — Government Grade 11", "Pre-Colonial Political Systems in West Africa", 6, new Guid("c5ad46a4-1f10-7aab-6b9d-9d32bca37e63") },
                    { new Guid("7fa3504e-0b0d-298e-1c01-859202a6b422"), "Introduction to Money and Banking — Economics Grade 10", "Introduction to Money and Banking", 9, new Guid("94f51606-1b58-9322-ecb2-cc8be90adce0") },
                    { new Guid("814810b4-f8d2-85d6-90ee-fa5e3136f220"), "Introduction to Commerce and Trade — Commerce Grade 10", "Introduction to Commerce and Trade", 1, new Guid("0ba6c666-e140-2220-8ec4-4f16bb2cd61b") },
                    { new Guid("8194a10d-fe45-7994-3a1c-552f9b37b7ba"), "Propagation of Flowering Plants — Floriculture Grade 10", "Propagation of Flowering Plants", 5, new Guid("4b421fd5-92e6-ef02-edd7-7804412bedd0") },
                    { new Guid("81a24229-7e92-9ae6-a61d-c3bdff2f3df6"), "Multimedia Concepts — Computer Studies Grade 11", "Multimedia Concepts", 10, new Guid("fffdd2fc-d6d3-7308-9d25-267ff832fd7f") },
                    { new Guid("82e11fb2-ec88-257f-f40e-5a3fa7dc4ebf"), "Road to Independence (The Gambia, Ghana, Nigeria) — History Grade 11", "Road to Independence (The Gambia, Ghana, Nigeria)", 5, new Guid("a33a76d9-ae1b-fb53-fef6-83cfc12992bb") },
                    { new Guid("839f6aa9-3188-83b6-2ae7-f038ff888a28"), "Classification of Flowers and Ornamental Plants — Floriculture Grade 10", "Classification of Flowers and Ornamental Plants", 2, new Guid("4b421fd5-92e6-ef02-edd7-7804412bedd0") },
                    { new Guid("83eef78b-f763-c601-e5d9-b923459cbd07"), "Sets and Binary Operations — Further Mathematics Grade 10", "Sets and Binary Operations", 1, new Guid("a4eaeeb3-b7a3-9abd-1c53-5309c8d473c0") },
                    { new Guid("84ab693e-8a82-c265-ed22-13fa4e91211f"), "Argumentative and Expository Essay Writing — English Language Grade 11", "Argumentative and Expository Essay Writing", 5, new Guid("f081fc93-cdb6-c836-9743-149f52228aab") },
                    { new Guid("85072438-b10b-4b0c-5c46-f01237bd64b9"), "Contemporary Issues in African History — History Grade 12", "Contemporary Issues in African History", 9, new Guid("8c240df7-cbb8-b63b-c2fa-15ef2ff299d6") },
                    { new Guid("860f89aa-b3a7-dc15-ffe9-fdcd7d2f9595"), "International Trade Documents and Procedures — Commerce Grade 12", "International Trade Documents and Procedures", 1, new Guid("6e0a7776-4921-0851-f05d-2a9cfe334aad") },
                    { new Guid("8715610a-99df-d8ac-f78d-18917b54895b"), "System Development Life Cycle (SDLC) — Computer Studies Grade 12", "System Development Life Cycle (SDLC)", 4, new Guid("30f1307e-bbbc-d420-61c7-124673a9fd1c") },
                    { new Guid("87665af0-d4ef-0687-63c4-4043b3cdd516"), "Introduction to Programming (Concepts, Flowcharts) — Computer Studies Grade 11", "Introduction to Programming (Concepts, Flowcharts)", 4, new Guid("fffdd2fc-d6d3-7308-9d25-267ff832fd7f") },
                    { new Guid("87c41ba9-26bc-a632-76e3-39f1845966ba"), "Local Government Administration — Government Grade 11", "Local Government Administration", 1, new Guid("c5ad46a4-1f10-7aab-6b9d-9d32bca37e63") },
                    { new Guid("88c2627c-9bc9-f1a7-046d-f8785bb3d537"), "Company Accounts (Shares, Debentures) — Financial Accounting Grade 12", "Company Accounts (Shares, Debentures)", 1, new Guid("4d512f11-4373-8e2f-543c-0842a91f4cf2") },
                    { new Guid("895ced0e-9390-528f-f2b3-213a050955e5"), "Oral English (Stress and Intonation Patterns) — English Language Grade 11", "Oral English (Stress and Intonation Patterns)", 7, new Guid("f081fc93-cdb6-c836-9743-149f52228aab") },
                    { new Guid("89de3736-f73e-15fb-2a12-f88f3e1aa006"), "Differentiation (Limits, Techniques, Applications) — Further Mathematics Grade 11", "Differentiation (Limits, Techniques, Applications)", 4, new Guid("a115558a-224b-bd96-8793-629375a2869e") },
                    { new Guid("8a056353-85cc-4012-36ac-edea89a35dd2"), "Fish Breeding and Hatchery Management — Fisheries Grade 11", "Fish Breeding and Hatchery Management", 3, new Guid("77d36d32-edfb-63a5-b573-c9b18f58bb17") },
                    { new Guid("8a38e09f-52b3-da07-3601-5f29bccfa6ee"), "Organic Horticulture and Sustainable Practices — Horticulture Grade 12", "Organic Horticulture and Sustainable Practices", 5, new Guid("e868e7f9-be26-b7fe-6bff-f3ca7fd9b926") },
                    { new Guid("8b666b2c-5e8e-a0c4-17c2-8b6a9ffc67cb"), "Vectors (Scalar and Vector Products) — Further Mathematics Grade 12", "Vectors (Scalar and Vector Products)", 6, new Guid("c0eae66d-d3fd-8079-f377-be72f93a2d06") },
                    { new Guid("8c009404-b43f-6103-4cf8-3fa516251337"), "Coordinate Geometry (Advanced) — Further Mathematics Grade 10", "Coordinate Geometry (Advanced)", 9, new Guid("a4eaeeb3-b7a3-9abd-1c53-5309c8d473c0") },
                    { new Guid("8c0221e3-b2fa-de12-daf8-d94fc88082bd"), "The Epistles (Pauline and General Letters) — Christian Religious Studies Grade 11", "The Epistles (Pauline and General Letters)", 7, new Guid("97ec5abf-b57f-0843-fe64-8f31af424c93") },
                    { new Guid("8c5ffbcb-913b-da9b-9f2b-6b82d8d29230"), "Islamic Contributions to Civilisation — Islamic Religious Studies Grade 11", "Islamic Contributions to Civilisation", 9, new Guid("029f27a3-acc3-f8ab-b362-4a0d9c1db6f6") },
                    { new Guid("8c96db37-f2ed-5000-f07e-ba8120e2e73a"), "World War I and II (Impact on Africa) — History Grade 11", "World War I and II (Impact on Africa)", 6, new Guid("a33a76d9-ae1b-fb53-fef6-83cfc12992bb") },
                    { new Guid("8ccff01a-a58a-eca0-607a-af7afa55ceb2"), "Mechanics (Statics, Friction, Equilibrium) — Further Mathematics Grade 12", "Mechanics (Statics, Friction, Equilibrium)", 2, new Guid("c0eae66d-d3fd-8079-f377-be72f93a2d06") },
                    { new Guid("8d914fd0-c570-35bf-b699-ab0469471992"), "Mechanics (Dynamics, Projectiles) — Further Mathematics Grade 12", "Mechanics (Dynamics, Projectiles)", 1, new Guid("c0eae66d-d3fd-8079-f377-be72f93a2d06") },
                    { new Guid("8d919dc1-b69c-c1df-b4e1-c1cdb6e1cb93"), "The Gambia Before Colonial Rule — History Grade 10", "The Gambia Before Colonial Rule", 9, new Guid("18c23abd-93c6-0e94-492c-dfd6d0a2ce28") },
                    { new Guid("8dc572e0-60d8-6190-78b1-10872bd512fe"), "Hire Purchase and Instalment Accounts — Financial Accounting Grade 12", "Hire Purchase and Instalment Accounts", 6, new Guid("4d512f11-4373-8e2f-543c-0842a91f4cf2") },
                    { new Guid("8e62aaf4-eac6-7240-e63e-9dca9775ad9f"), "The Gambian Independence Movement — History Grade 11", "The Gambian Independence Movement", 8, new Guid("a33a76d9-ae1b-fb53-fef6-83cfc12992bb") },
                    { new Guid("8ee08337-5195-8a47-c4c9-a5f126ce6b2a"), "Challenges of Governance in West Africa — Government Grade 12", "Challenges of Governance in West Africa", 7, new Guid("661ec45c-8411-3167-8af8-544152cebce6") },
                    { new Guid("90a962b4-89f3-9513-701c-591adf448cc2"), "Economic Integration and Trade Blocs — Commerce Grade 12", "Economic Integration and Trade Blocs", 3, new Guid("6e0a7776-4921-0851-f05d-2a9cfe334aad") },
                    { new Guid("9198b903-0ae3-9408-6761-e898208d6df6"), "Conflicts, Resolution and Peace-building — Government Grade 12", "Conflicts, Resolution and Peace-building", 5, new Guid("661ec45c-8411-3167-8af8-544152cebce6") },
                    { new Guid("92b0f620-8a11-9591-ed1c-54fd9d9e9262"), "Unemployment and Poverty Alleviation — Economics Grade 12", "Unemployment and Poverty Alleviation", 7, new Guid("0d331ad8-0343-b862-4378-b5f017817a63") },
                    { new Guid("93fc90dc-2ae3-fa31-1bb2-7a98f6a86e89"), "Oral English (Vowel and Consonant Sounds) — English Language Grade 10", "Oral English (Vowel and Consonant Sounds)", 8, new Guid("def2214a-a301-249e-c656-0914b4ecb5a5") },
                    { new Guid("947dae29-0291-7252-b08c-d0db61a9ed94"), "Transportation and Communication — Economics Grade 11", "Transportation and Communication", 9, new Guid("04fbf1c3-6b48-d6c9-e60b-8af777b7fa50") },
                    { new Guid("9656578b-7068-fd94-1ce4-d46cd73b4177"), "Commercial Flower Production and Marketing — Floriculture Grade 12", "Commercial Flower Production and Marketing", 1, new Guid("f1ddfb49-3ee7-d0d0-3bdb-ba71e5d13aec") },
                    { new Guid("96724fb6-a875-66d8-9360-63c21e88f34d"), "Rule of Law and Constitutionalism — Government Grade 10", "Rule of Law and Constitutionalism", 8, new Guid("c0e3b77a-b7e6-f84c-1414-a9b4986364c7") },
                    { new Guid("9862fe5f-522d-24dd-ca89-ada7e91f9bba"), "National Income (Measurement and Concepts) — Economics Grade 11", "National Income (Measurement and Concepts)", 2, new Guid("04fbf1c3-6b48-d6c9-e60b-8af777b7fa50") },
                    { new Guid("98870757-a079-6964-80c2-495781f40570"), "Flower Production and Management — Horticulture Grade 11", "Flower Production and Management", 4, new Guid("efa84916-91b0-0dea-73a8-860fc9a80941") },
                    { new Guid("992316f5-4339-b9c7-3785-f4dfeae92c31"), "Complex Numbers — Further Mathematics Grade 11", "Complex Numbers", 1, new Guid("a115558a-224b-bd96-8793-629375a2869e") },
                    { new Guid("999b9136-8eb9-3918-cac3-c5d2e12738be"), "Parts of Speech and Sentence Structure — English Language Grade 10", "Parts of Speech and Sentence Structure", 4, new Guid("def2214a-a301-249e-c656-0914b4ecb5a5") },
                    { new Guid("9d33aa52-aa70-5ff4-13ac-b08642f0871d"), "Population and Labour Force — Economics Grade 10", "Population and Labour Force", 8, new Guid("94f51606-1b58-9322-ecb2-cc8be90adce0") },
                    { new Guid("9d6759f8-0de0-f6e8-3d29-dd837aee70d2"), "Principles of Democracy — Government Grade 10", "Principles of Democracy", 4, new Guid("c0e3b77a-b7e6-f84c-1414-a9b4986364c7") },
                    { new Guid("9ee931fe-12bb-d6bb-8701-90fed351a761"), "Zakat, Sawm and Hajj (In Detail) — Islamic Religious Studies Grade 11", "Zakat, Sawm and Hajj (In Detail)", 3, new Guid("029f27a3-acc3-f8ab-b362-4a0d9c1db6f6") },
                    { new Guid("9ef2f6f9-331d-3b10-8f59-f03329a9e76e"), "Careers in Floriculture and Landscaping — Floriculture Grade 12", "Careers in Floriculture and Landscaping", 9, new Guid("f1ddfb49-3ee7-d0d0-3bdb-ba71e5d13aec") },
                    { new Guid("9f5aff00-a15b-85bb-d775-a3f8c46ed2b4"), "Mathematical Modelling — Further Mathematics Grade 12", "Mathematical Modelling", 9, new Guid("c0eae66d-d3fd-8079-f377-be72f93a2d06") },
                    { new Guid("9fa153e2-0380-6e52-7478-b567e580e218"), "Basic Concepts in Government (State, Power, Authority) — Government Grade 10", "Basic Concepts in Government (State, Power, Authority)", 1, new Guid("c0e3b77a-b7e6-f84c-1414-a9b4986364c7") },
                    { new Guid("9fe7f7a2-59dd-52c3-4a6a-e0eb1e78db50"), "Greenhouse and Protected Cultivation — Horticulture Grade 11", "Greenhouse and Protected Cultivation", 2, new Guid("efa84916-91b0-0dea-73a8-860fc9a80941") },
                    { new Guid("a04550cd-cbaf-ef14-8333-0b5ae28557dd"), "Plantation Crop Production (Cashew, Oil Palm, Cocoa) — Horticulture Grade 12", "Plantation Crop Production (Cashew, Oil Palm, Cocoa)", 3, new Guid("e868e7f9-be26-b7fe-6bff-f3ca7fd9b926") },
                    { new Guid("a4ce9914-e83c-3326-7ce6-a93212e6379b"), "WASSCE Past Questions Practice — Further Mathematics Grade 12", "WASSCE Past Questions Practice", 10, new Guid("c0eae66d-d3fd-8079-f377-be72f93a2d06") },
                    { new Guid("a4d05415-560c-c6ba-9ff6-0652e55c71f7"), "Database Management (Microsoft Access) — Computer Studies Grade 11", "Database Management (Microsoft Access)", 2, new Guid("fffdd2fc-d6d3-7308-9d25-267ff832fd7f") },
                    { new Guid("a58ec9a5-80b4-6000-6934-5490d702ed98"), "The Cold War and Its Impact on Africa — History Grade 12", "The Cold War and Its Impact on Africa", 6, new Guid("8c240df7-cbb8-b63b-c2fa-15ef2ff299d6") },
                    { new Guid("a593e80a-cdb9-08f0-9452-0b58678067fe"), "Economic Development Since Independence — History Grade 12", "Economic Development Since Independence", 8, new Guid("8c240df7-cbb8-b63b-c2fa-15ef2ff299d6") },
                    { new Guid("a5e87ac5-7183-3cc2-6883-ecc75da8df01"), "Garden Tools and Equipment — Horticulture Grade 10", "Garden Tools and Equipment", 8, new Guid("3c22945c-cc6c-6199-f614-746cb565217c") },
                    { new Guid("a5ea5a3d-3c93-57dd-40f8-f5d81595866c"), "Theory of Consumer Behaviour — Economics Grade 10", "Theory of Consumer Behaviour", 4, new Guid("94f51606-1b58-9322-ecb2-cc8be90adce0") },
                    { new Guid("a635fc05-f3bc-9bd3-1fe3-423d3c3a19bb"), "WASSCE Past Questions Practice — History Grade 12", "WASSCE Past Questions Practice", 10, new Guid("8c240df7-cbb8-b63b-c2fa-15ef2ff299d6") },
                    { new Guid("a6406211-28c3-2103-9e94-fecf6bd49102"), "Islam in West Africa (History and Spread) — Islamic Religious Studies Grade 11", "Islam in West Africa (History and Spread)", 8, new Guid("029f27a3-acc3-f8ab-b362-4a0d9c1db6f6") },
                    { new Guid("a6e1aa10-686e-c1cc-3f3a-76baca6bb892"), "Islamic Morals and Manners (Akhlaq) — Islamic Religious Studies Grade 10", "Islamic Morals and Manners (Akhlaq)", 9, new Guid("598041c3-b226-5552-6f56-7b081f4d8279") },
                    { new Guid("a70616d7-4a38-b49b-84fe-591f1a1a3d49"), "Statistics (Regression, Correlation) — Further Mathematics Grade 11", "Statistics (Regression, Correlation)", 9, new Guid("a115558a-224b-bd96-8793-629375a2869e") },
                    { new Guid("a839cfde-ad9c-4754-4e18-f52be917be2f"), "Commercial Law (Contracts, Agency) — Commerce Grade 12", "Commercial Law (Contracts, Agency)", 9, new Guid("6e0a7776-4921-0851-f05d-2a9cfe334aad") },
                    { new Guid("a86df0b0-1f48-0c48-8452-c23269cd9e6d"), "WASSCE Past Questions Practice — Commerce Grade 12", "WASSCE Past Questions Practice", 10, new Guid("6e0a7776-4921-0851-f05d-2a9cfe334aad") },
                    { new Guid("a90fb801-483f-a6cc-d597-df04e56ad6c5"), "Differential Equations (First Order) — Further Mathematics Grade 12", "Differential Equations (First Order)", 3, new Guid("c0eae66d-d3fd-8079-f377-be72f93a2d06") },
                    { new Guid("aa171a3e-aca0-17be-50e2-21c3e5c41522"), "The Six Articles of Faith (Iman) — Islamic Religious Studies Grade 10", "The Six Articles of Faith (Iman)", 5, new Guid("598041c3-b226-5552-6f56-7b081f4d8279") },
                    { new Guid("ab8f974c-a503-1789-4d35-8f6d8b6be739"), "Register and Style — English Language Grade 11", "Register and Style", 9, new Guid("f081fc93-cdb6-c836-9743-149f52228aab") },
                    { new Guid("abd3382e-ecd2-3a0a-7898-4190e752efcb"), "Introduction to the New Testament — Christian Religious Studies Grade 10", "Introduction to the New Testament", 9, new Guid("35b0b395-4f39-535e-d197-cb82f839fbbc") },
                    { new Guid("abfea02b-7c76-77f7-f39c-c58a20db6fee"), "ECOWAS and Regional Cooperation — History Grade 12", "ECOWAS and Regional Cooperation", 3, new Guid("8c240df7-cbb8-b63b-c2fa-15ef2ff299d6") },
                    { new Guid("ac4cf89c-49aa-ecf4-27c1-95a27c1cd0d6"), "Christianity and African Traditional Religion — Christian Religious Studies Grade 12", "Christianity and African Traditional Religion", 2, new Guid("30f59780-4140-3f0a-cded-8b13fd7052cb") },
                    { new Guid("ad544a15-a25d-1735-8c18-bbd644cf4386"), "Foreign Trade (Imports, Exports, Entreports) — Commerce Grade 10", "Foreign Trade (Imports, Exports, Entreports)", 3, new Guid("0ba6c666-e140-2220-8ec4-4f16bb2cd61b") },
                    { new Guid("ad65bfcc-86c2-e95e-3ec7-a835fa0b03a7"), "Accounts from Incomplete Records (Advanced) — Financial Accounting Grade 12", "Accounts from Incomplete Records (Advanced)", 4, new Guid("4d512f11-4373-8e2f-543c-0842a91f4cf2") },
                    { new Guid("ada8d8a9-46a2-4fac-2b4c-79c6400f668c"), "The Church (Nature, Unity, Mission) — Christian Religious Studies Grade 12", "The Church (Nature, Unity, Mission)", 1, new Guid("30f59780-4140-3f0a-cded-8b13fd7052cb") },
                    { new Guid("aebc62aa-74ee-ce2d-9be6-53db787f76aa"), "Introduction to Book-Keeping and Accounting — Financial Accounting Grade 10", "Introduction to Book-Keeping and Accounting", 1, new Guid("cd368ced-9b4b-53c2-f818-261143f97c4b") },
                    { new Guid("af405fbd-33bc-f9bf-9d33-99eb3f265837"), "Organic Flower Production — Floriculture Grade 12", "Organic Flower Production", 7, new Guid("f1ddfb49-3ee7-d0d0-3bdb-ba71e5d13aec") },
                    { new Guid("b0064c3e-37d1-c177-b47e-06643aa167ae"), "WASSCE Past Questions Practice — Government Grade 12", "WASSCE Past Questions Practice", 10, new Guid("661ec45c-8411-3167-8af8-544152cebce6") },
                    { new Guid("b0f7123e-5997-26c6-bd4e-ac1806213727"), "Narrative and Descriptive Essay Writing — English Language Grade 10", "Narrative and Descriptive Essay Writing", 6, new Guid("def2214a-a301-249e-c656-0914b4ecb5a5") },
                    { new Guid("b19a1355-f187-06b0-cc62-901e7b216097"), "Balance of Payments — Economics Grade 11", "Balance of Payments", 10, new Guid("04fbf1c3-6b48-d6c9-e60b-8af777b7fa50") },
                    { new Guid("b1d7d357-02e3-ee1a-5ca8-16818c591fff"), "Political Parties and Pressure Groups — Government Grade 10", "Political Parties and Pressure Groups", 9, new Guid("c0e3b77a-b7e6-f84c-1414-a9b4986364c7") },
                    { new Guid("b27072b9-04b8-446e-53c1-29b19c3de8ea"), "Pest and Disease Management in Gardens — Horticulture Grade 10", "Pest and Disease Management in Gardens", 10, new Guid("3c22945c-cc6c-6199-f614-746cb565217c") },
                    { new Guid("b5048787-a638-c63b-3c4f-0388e11ce8d5"), "Privatisation and Commercialisation — Economics Grade 12", "Privatisation and Commercialisation", 8, new Guid("0d331ad8-0343-b862-4378-b5f017817a63") },
                    { new Guid("b5a068e9-228d-6c5a-cf32-5c3bb1d25eab"), "Environmental Issues in Horticulture — Horticulture Grade 12", "Environmental Issues in Horticulture", 9, new Guid("e868e7f9-be26-b7fe-6bff-f3ca7fd9b926") },
                    { new Guid("b645e12e-4305-ced3-f9aa-1aca899b16c1"), "The Five Pillars of Islam — Islamic Religious Studies Grade 10", "The Five Pillars of Islam", 4, new Guid("598041c3-b226-5552-6f56-7b081f4d8279") },
                    { new Guid("b8dc7f3f-024a-4b89-8877-64fb05930ef2"), "Partnership Accounts (Formation, Profit Sharing) — Financial Accounting Grade 11", "Partnership Accounts (Formation, Profit Sharing)", 7, new Guid("c33eef6f-8e20-9183-f5a2-4e5187de42d7") },
                    { new Guid("b9422bef-1ece-44f4-27bf-480b5a0377c5"), "Spreadsheets (Microsoft Excel, Formulas, Charts) — Computer Studies Grade 11", "Spreadsheets (Microsoft Excel, Formulas, Charts)", 1, new Guid("fffdd2fc-d6d3-7308-9d25-267ff832fd7f") },
                    { new Guid("b9ecd87c-9681-9a46-6086-2115b1d6c3a7"), "WASSCE Past Questions Practice — Economics Grade 12", "WASSCE Past Questions Practice", 10, new Guid("0d331ad8-0343-b862-4378-b5f017817a63") },
                    { new Guid("ba8edbf2-73b4-1b86-a489-41bdf1ffc477"), "Conflict and Conflict Resolution in West Africa — History Grade 12", "Conflict and Conflict Resolution in West Africa", 7, new Guid("8c240df7-cbb8-b63b-c2fa-15ef2ff299d6") },
                    { new Guid("babb95d7-5db9-2f96-e1d6-8009ccbf15ab"), "Aids to Trade (Overview) — Commerce Grade 10", "Aids to Trade (Overview)", 4, new Guid("0ba6c666-e140-2220-8ec4-4f16bb2cd61b") },
                    { new Guid("bb439a1b-1ed7-ccdb-aedf-3e38c71d67a5"), "Comprehension (Passages and Inference) — English Language Grade 10", "Comprehension (Passages and Inference)", 1, new Guid("def2214a-a301-249e-c656-0914b4ecb5a5") },
                    { new Guid("bb54aaea-eb49-042c-8bf7-46f2155d2adb"), "Fish Nutrition and Feeding — Fisheries Grade 10", "Fish Nutrition and Feeding", 5, new Guid("70a66f3c-d5f9-1b35-bc9c-119ef3d85f3e") },
                    { new Guid("bcf81166-e945-7d8b-fb42-18efbfedf1c1"), "Reading Skills and Techniques — English Language Grade 10", "Reading Skills and Techniques", 9, new Guid("def2214a-a301-249e-c656-0914b4ecb5a5") },
                    { new Guid("bdb5b4b9-db40-b972-0ef7-474cc9e5f65c"), "Fisheries Economics and Marketing — Fisheries Grade 12", "Fisheries Economics and Marketing", 3, new Guid("aea3fc29-a3f7-76c0-8be7-f73335d78889") },
                    { new Guid("bdbfc482-07ad-6a18-4842-87452cd599c1"), "Inflation and Deflation — Economics Grade 11", "Inflation and Deflation", 4, new Guid("04fbf1c3-6b48-d6c9-e60b-8af777b7fa50") },
                    { new Guid("bde1c280-6b64-aacf-70f5-aa35a10fe219"), "The Judges of Israel — Christian Religious Studies Grade 10", "The Judges of Israel", 5, new Guid("35b0b395-4f39-535e-d197-cb82f839fbbc") },
                    { new Guid("be1c942c-d410-b07c-c413-123a78d1676e"), "Greenhouse Technology for Flower Production — Floriculture Grade 11", "Greenhouse Technology for Flower Production", 2, new Guid("49841f61-c212-8eec-2810-f1bb824e37d2") },
                    { new Guid("bead5d43-7846-3e45-5b85-6e832d7dfb32"), "Introduction to Fisheries (Scope, Importance) — Fisheries Grade 10", "Introduction to Fisheries (Scope, Importance)", 1, new Guid("70a66f3c-d5f9-1b35-bc9c-119ef3d85f3e") },
                    { new Guid("bec8a9d6-0d6c-9fb6-5dec-4e8ec8be31bf"), "Vegetable Seed Production — Horticulture Grade 11", "Vegetable Seed Production", 6, new Guid("efa84916-91b0-0dea-73a8-860fc9a80941") },
                    { new Guid("bed34e7e-dd98-3b56-5f90-b21e3fdc40c6"), "WASSCE Past Questions Practice — Floriculture Grade 12", "WASSCE Past Questions Practice", 10, new Guid("f1ddfb49-3ee7-d0d0-3bdb-ba71e5d13aec") },
                    { new Guid("bee7a0d1-3f25-2a8e-01fc-357a8352a0c2"), "Socio-Economic Importance of Fisheries — Fisheries Grade 11", "Socio-Economic Importance of Fisheries", 10, new Guid("77d36d32-edfb-63a5-b573-c9b18f58bb17") },
                    { new Guid("bf08b4e6-b503-e717-9652-bb656a48782e"), "Theory of Production — Economics Grade 10", "Theory of Production", 5, new Guid("94f51606-1b58-9322-ecb2-cc8be90adce0") },
                    { new Guid("bf587877-8deb-da73-92b2-0ec60d694764"), "E-Commerce and Digital Business — Commerce Grade 12", "E-Commerce and Digital Business", 4, new Guid("6e0a7776-4921-0851-f05d-2a9cfe334aad") },
                    { new Guid("bf5ea608-8c78-902f-a073-f5882366c019"), "WASSCE Past Questions Practice — Fisheries Grade 12", "WASSCE Past Questions Practice", 10, new Guid("aea3fc29-a3f7-76c0-8be7-f73335d78889") },
                    { new Guid("c06ff719-754b-d6d5-f4ed-da66924fb992"), "Hadith Studies (Selected Ahadith) — Islamic Religious Studies Grade 11", "Hadith Studies (Selected Ahadith)", 7, new Guid("029f27a3-acc3-f8ab-b362-4a0d9c1db6f6") },
                    { new Guid("c085ba58-1b9d-b425-aa3d-d8183270ce74"), "Military Interventions in West Africa — History Grade 11", "Military Interventions in West Africa", 10, new Guid("a33a76d9-ae1b-fb53-fef6-83cfc12992bb") },
                    { new Guid("c1976dc6-d9c7-b3bd-173b-d408b4b0b268"), "Islam and Science, Technology and Development — Islamic Religious Studies Grade 12", "Islam and Science, Technology and Development", 8, new Guid("0a0e509f-a99c-3193-c277-928d54224cc2") },
                    { new Guid("c34bb655-bee4-eedf-7356-a6fb788c169a"), "Computer Maintenance and Troubleshooting — Computer Studies Grade 12", "Computer Maintenance and Troubleshooting", 9, new Guid("30f1307e-bbbc-d420-61c7-124673a9fd1c") },
                    { new Guid("c43e735d-dbd7-5598-40ef-06f45fe7ccbe"), "Globalisation and Economic Growth — Economics Grade 12", "Globalisation and Economic Growth", 6, new Guid("0d331ad8-0343-b862-4378-b5f017817a63") },
                    { new Guid("c48f92bf-ec87-8a06-6a13-027bd483ec17"), "Marketing (Concepts, Functions, Mix) — Commerce Grade 11", "Marketing (Concepts, Functions, Mix)", 2, new Guid("773db100-257e-8d22-3695-0e647cbb7fd4") },
                    { new Guid("c4ed813a-958d-2f3c-0d62-1119ede89ffd"), "Operating Systems (Functions, Types) — Computer Studies Grade 10", "Operating Systems (Functions, Types)", 5, new Guid("43daec58-ceb4-fc4f-f1d4-086bd79d72a8") },
                    { new Guid("c606033e-b2dc-67f4-fd5e-a602b8f09d85"), "Apartheid and Liberation in Southern Africa — History Grade 12", "Apartheid and Liberation in Southern Africa", 4, new Guid("8c240df7-cbb8-b63b-c2fa-15ef2ff299d6") },
                    { new Guid("c65e15be-8b0c-51a7-fad2-c165f23ce233"), "The Acts of the Apostles (Early Church) — Christian Religious Studies Grade 11", "The Acts of the Apostles (Early Church)", 5, new Guid("97ec5abf-b57f-0843-fe64-8f31af424c93") },
                    { new Guid("c689bc6e-80c3-6346-8bba-02665e1ac8dd"), "Introduction to Computers (History, Generations) — Computer Studies Grade 10", "Introduction to Computers (History, Generations)", 1, new Guid("43daec58-ceb4-fc4f-f1d4-086bd79d72a8") },
                    { new Guid("c68bcacf-7906-de5c-c7f3-c1d5b0e90ba5"), "Islamic Family Law (Marriage, Divorce, Inheritance) — Islamic Religious Studies Grade 11", "Islamic Family Law (Marriage, Divorce, Inheritance)", 5, new Guid("029f27a3-acc3-f8ab-b362-4a0d9c1db6f6") },
                    { new Guid("c6cefad6-c101-dcf5-25d3-cdcc69cfde5b"), "Provision for Bad and Doubtful Debts — Financial Accounting Grade 11", "Provision for Bad and Doubtful Debts", 3, new Guid("c33eef6f-8e20-9183-f5a2-4e5187de42d7") },
                    { new Guid("c75edf5b-0f18-35b6-08ca-4912b198d66d"), "The Gambian Economy (Overview) — Economics Grade 10", "The Gambian Economy (Overview)", 10, new Guid("94f51606-1b58-9322-ecb2-cc8be90adce0") },
                    { new Guid("c7af1d43-6649-54aa-bac8-91d91d828f8e"), "Corruption and Anti-Corruption Measures — Government Grade 12", "Corruption and Anti-Corruption Measures", 8, new Guid("661ec45c-8411-3167-8af8-544152cebce6") },
                    { new Guid("c844ee5f-24c8-9929-8254-27eb9d18f2aa"), "Post-Harvest Handling of Cut Flowers — Floriculture Grade 11", "Post-Harvest Handling of Cut Flowers", 8, new Guid("49841f61-c212-8eec-2810-f1bb824e37d2") },
                    { new Guid("c8d00f57-21f4-1e33-0f09-98b78a5e4c65"), "Literary Appreciation (Prose, Poetry, Drama) — English Language Grade 12", "Literary Appreciation (Prose, Poetry, Drama)", 9, new Guid("9e7459ec-d351-be24-197d-c29757abab72") },
                    { new Guid("cc31bbb7-6e90-b1e9-3316-c622caee97fd"), "Economic Problems of West Africa — Economics Grade 12", "Economic Problems of West Africa", 5, new Guid("0d331ad8-0343-b862-4378-b5f017817a63") },
                    { new Guid("cca4f5f7-2531-6503-4ee1-6f854789bf65"), "Permutations and Combinations — Further Mathematics Grade 10", "Permutations and Combinations", 6, new Guid("a4eaeeb3-b7a3-9abd-1c53-5309c8d473c0") },
                    { new Guid("cd11e8e6-1d73-998c-9801-d39ec3ad537b"), "Eschatology (Last Things, Revelation) — Christian Religious Studies Grade 12", "Eschatology (Last Things, Revelation)", 9, new Guid("30f59780-4140-3f0a-cded-8b13fd7052cb") },
                    { new Guid("cd6813fd-d843-35c4-e1a3-cbb03e5a3bed"), "Artificial Intelligence and Emerging Technologies — Computer Studies Grade 12", "Artificial Intelligence and Emerging Technologies", 7, new Guid("30f1307e-bbbc-d420-61c7-124673a9fd1c") },
                    { new Guid("cdfe0db6-66ba-42f0-baef-e35200f8e3e3"), "Environmental Economics — Economics Grade 12", "Environmental Economics", 9, new Guid("0d331ad8-0343-b862-4378-b5f017817a63") },
                    { new Guid("ce2ead18-c9f3-72be-5d36-d46b15699b80"), "Islamic Political System (Shura, Caliphate) — Islamic Religious Studies Grade 12", "Islamic Political System (Shura, Caliphate)", 7, new Guid("0a0e509f-a99c-3193-c277-928d54224cc2") },
                    { new Guid("cfb2cb53-4481-3dd4-a97b-35daece3b89b"), "Clause Analysis and Complex Sentences — English Language Grade 11", "Clause Analysis and Complex Sentences", 4, new Guid("f081fc93-cdb6-c836-9743-149f52228aab") },
                    { new Guid("cfbf6b22-f2f6-dd90-9c68-0f00c2a432d7"), "Water Quality and Management — Fisheries Grade 10", "Water Quality and Management", 9, new Guid("70a66f3c-d5f9-1b35-bc9c-119ef3d85f3e") },
                    { new Guid("d078acd6-71c5-bac9-9e1d-59508f845a5e"), "Overview of West African Empires — History Grade 10", "Overview of West African Empires", 10, new Guid("18c23abd-93c6-0e94-492c-dfd6d0a2ce28") },
                    { new Guid("d33c69f4-4f9f-b665-4466-933f01d264e1"), "Environmental Impact of Floriculture — Floriculture Grade 12", "Environmental Impact of Floriculture", 6, new Guid("f1ddfb49-3ee7-d0d0-3bdb-ba71e5d13aec") },
                    { new Guid("d376a968-f982-3fc3-ca70-2e6c670ccd19"), "The Ledger (Personal, Real, Nominal Accounts) — Financial Accounting Grade 10", "The Ledger (Personal, Real, Nominal Accounts)", 4, new Guid("cd368ced-9b4b-53c2-f818-261143f97c4b") },
                    { new Guid("d3ebf823-652b-837f-cccb-c0dcec37942c"), "Urban and Peri-Urban Horticulture — Horticulture Grade 12", "Urban and Peri-Urban Horticulture", 4, new Guid("e868e7f9-be26-b7fe-6bff-f3ca7fd9b926") },
                    { new Guid("d58209ae-5619-7cb6-8a15-25db24f7f6e8"), "Transportation (Modes, Importance) — Commerce Grade 10", "Transportation (Modes, Importance)", 8, new Guid("0ba6c666-e140-2220-8ec4-4f16bb2cd61b") },
                    { new Guid("d5e88fcd-5628-845a-6cd3-817582c02f23"), "Control Accounts (Sales and Purchases Ledger) — Financial Accounting Grade 11", "Control Accounts (Sales and Purchases Ledger)", 1, new Guid("c33eef6f-8e20-9183-f5a2-4e5187de42d7") },
                    { new Guid("d63c256c-f0c2-d71a-9d8e-8d70ecdfa3b4"), "Introduction to Floriculture (Scope and Importance) — Floriculture Grade 10", "Introduction to Floriculture (Scope and Importance)", 1, new Guid("4b421fd5-92e6-ef02-edd7-7804412bedd0") },
                    { new Guid("d687c787-b771-df5f-ed0e-31154b09649d"), "Comprehension (WASSCE Paper 1 Practice) — English Language Grade 12", "Comprehension (WASSCE Paper 1 Practice)", 1, new Guid("9e7459ec-d351-be24-197d-c29757abab72") },
                    { new Guid("d6f0d4cb-ebca-9957-0e76-5e77799ac336"), "Introduction to Aquaculture — Fisheries Grade 10", "Introduction to Aquaculture", 10, new Guid("70a66f3c-d5f9-1b35-bc9c-119ef3d85f3e") },
                    { new Guid("d7ea2f0c-a109-d8c4-7ba0-ec13add19fe0"), "Binomial Theorem — Further Mathematics Grade 10", "Binomial Theorem", 7, new Guid("a4eaeeb3-b7a3-9abd-1c53-5309c8d473c0") },
                    { new Guid("d815426d-017e-c103-b345-771e519fbf62"), "Fiscal and Monetary Policy — Economics Grade 11", "Fiscal and Monetary Policy", 5, new Guid("04fbf1c3-6b48-d6c9-e60b-8af777b7fa50") },
                    { new Guid("d86a8dbe-6b19-80b9-033f-feb4f48159ca"), "The United Monarchy (Saul, David, Solomon) — Christian Religious Studies Grade 10", "The United Monarchy (Saul, David, Solomon)", 6, new Guid("35b0b395-4f39-535e-d197-cb82f839fbbc") },
                    { new Guid("d8f0b4c8-b43e-c0d7-0b51-d7c462430014"), "Source Documents and Books of Original Entry — Financial Accounting Grade 10", "Source Documents and Books of Original Entry", 2, new Guid("cd368ced-9b4b-53c2-f818-261143f97c4b") },
                    { new Guid("da6e9201-055a-dba8-12ad-fd85745dba81"), "WASSCE Past Questions Practice — Islamic Religious Studies Grade 12", "WASSCE Past Questions Practice", 10, new Guid("0a0e509f-a99c-3193-c277-928d54224cc2") },
                    { new Guid("da8d12d4-acbc-8fe1-68c7-f60adb34a66d"), "Data Processing (Batch, Real-Time, Online) — Computer Studies Grade 11", "Data Processing (Batch, Real-Time, Online)", 8, new Guid("fffdd2fc-d6d3-7308-9d25-267ff832fd7f") },
                    { new Guid("dc54127c-9220-0036-58b9-6e27b0ab7a07"), "Fisheries in The Gambia (Overview) — Fisheries Grade 10", "Fisheries in The Gambia (Overview)", 8, new Guid("70a66f3c-d5f9-1b35-bc9c-119ef3d85f3e") },
                    { new Guid("dc6f8665-ef31-7ee0-0282-3e86812f7f37"), "Floral Design and Arrangement — Floriculture Grade 11", "Floral Design and Arrangement", 4, new Guid("49841f61-c212-8eec-2810-f1bb824e37d2") },
                    { new Guid("dcd81206-f567-43f8-2d58-62ce548cee37"), "Non-Governmental Organisations (NGOs) — Government Grade 12", "Non-Governmental Organisations (NGOs)", 4, new Guid("661ec45c-8411-3167-8af8-544152cebce6") },
                    { new Guid("dd6fcfc9-5873-af97-4a8f-f69f51265ae7"), "Common Cut Flowers (Types, Growing Conditions) — Floriculture Grade 10", "Common Cut Flowers (Types, Growing Conditions)", 7, new Guid("4b421fd5-92e6-ef02-edd7-7804412bedd0") },
                    { new Guid("ddcafbfe-400b-3369-ae82-5679a11bea6f"), "Introduction to Fiqh (Islamic Jurisprudence) — Islamic Religious Studies Grade 10", "Introduction to Fiqh (Islamic Jurisprudence)", 10, new Guid("598041c3-b226-5552-6f56-7b081f4d8279") },
                    { new Guid("de647112-e4fc-1469-3e2f-766f1b8c47a5"), "Trade Associations and Chambers of Commerce — Commerce Grade 11", "Trade Associations and Chambers of Commerce", 8, new Guid("773db100-257e-8d22-3695-0e647cbb7fd4") },
                    { new Guid("df731687-b4cc-2267-9639-d3bcd8461547"), "Orchard Management and Fruit Tree Care — Horticulture Grade 12", "Orchard Management and Fruit Tree Care", 2, new Guid("e868e7f9-be26-b7fe-6bff-f3ca7fd9b926") },
                    { new Guid("df81ed8e-bc16-2bcb-c53e-b1b686ac96a6"), "Double Entry System — Financial Accounting Grade 10", "Double Entry System", 3, new Guid("cd368ced-9b4b-53c2-f818-261143f97c4b") },
                    { new Guid("df9980c0-681c-d8be-682a-9b3839ef9298"), "Summary Writing — English Language Grade 10", "Summary Writing", 2, new Guid("def2214a-a301-249e-c656-0914b4ecb5a5") },
                    { new Guid("dfba7a2a-5c14-c802-17f5-7cfc0addff5b"), "Logical Reasoning — Further Mathematics Grade 10", "Logical Reasoning", 5, new Guid("a4eaeeb3-b7a3-9abd-1c53-5309c8d473c0") },
                    { new Guid("dfc4f7bc-f0dc-7dd0-392e-742da60583a4"), "Money (Functions, Qualities, Types) — Commerce Grade 10", "Money (Functions, Qualities, Types)", 5, new Guid("0ba6c666-e140-2220-8ec4-4f16bb2cd61b") },
                    { new Guid("e13ba512-5382-7fb9-cf1b-13c5007692ba"), "International Organisations (UN, AU, ECOWAS, Commonwealth) — Government Grade 12", "International Organisations (UN, AU, ECOWAS, Commonwealth)", 3, new Guid("661ec45c-8411-3167-8af8-544152cebce6") },
                    { new Guid("e17f6d51-92b7-9d6a-c9e9-85e3564c477a"), "Punctuation and Spelling — English Language Grade 10", "Punctuation and Spelling", 10, new Guid("def2214a-a301-249e-c656-0914b4ecb5a5") },
                    { new Guid("e1e58c26-b9d7-94cd-71f6-f1dce6da150b"), "Economic Integration (ECOWAS, AU) — Economics Grade 12", "Economic Integration (ECOWAS, AU)", 2, new Guid("0d331ad8-0343-b862-4378-b5f017817a63") },
                    { new Guid("e1fe9dfd-3ce5-a6fc-ddd1-1118d8c0ea20"), "Fisheries Management and Conservation — Fisheries Grade 11", "Fisheries Management and Conservation", 6, new Guid("77d36d32-edfb-63a5-b573-c9b18f58bb17") },
                    { new Guid("e294defc-e9e3-090f-54cc-e5708481d7fe"), "Government of The Gambia (Structure and Functions) — Government Grade 11", "Government of The Gambia (Structure and Functions)", 9, new Guid("c5ad46a4-1f10-7aab-6b9d-9d32bca37e63") },
                    { new Guid("e2fb33cd-7480-6439-ae51-4726a81c3520"), "Turf and Lawn Management — Floriculture Grade 11", "Turf and Lawn Management", 6, new Guid("49841f61-c212-8eec-2810-f1bb824e37d2") },
                    { new Guid("e3687575-b413-7d46-9893-2da6ab44671c"), "Money, Banking and Financial Institutions — Economics Grade 11", "Money, Banking and Financial Institutions", 3, new Guid("04fbf1c3-6b48-d6c9-e60b-8af777b7fa50") },
                    { new Guid("e36ba8e2-613c-c2ba-cc72-f470267cd53c"), "Balance Sheet — Financial Accounting Grade 10", "Balance Sheet", 8, new Guid("cd368ced-9b4b-53c2-f818-261143f97c4b") },
                    { new Guid("e428783e-6ef7-9528-df6e-4ff76b0bbb37"), "Word Processing (Microsoft Word) — Computer Studies Grade 10", "Word Processing (Microsoft Word)", 6, new Guid("43daec58-ceb4-fc4f-f1d4-086bd79d72a8") },
                    { new Guid("e63d564a-4a60-6673-d095-154feca8078e"), "Essay Writing (All Types — WASSCE Format) — English Language Grade 12", "Essay Writing (All Types — WASSCE Format)", 4, new Guid("9e7459ec-d351-be24-197d-c29757abab72") },
                    { new Guid("e68e52d4-4a80-6c8f-ed5f-a6100fc69453"), "Trading, Profit and Loss Account — Financial Accounting Grade 10", "Trading, Profit and Loss Account", 7, new Guid("cd368ced-9b4b-53c2-f818-261143f97c4b") },
                    { new Guid("e6b2e006-d050-cf51-e3dc-1835a18ab8ff"), "Correction of Errors — Financial Accounting Grade 10", "Correction of Errors", 10, new Guid("cd368ced-9b4b-53c2-f818-261143f97c4b") },
                    { new Guid("e749f9cc-b976-e972-5a4e-0517551e75a4"), "Plant Propagation (Seeds, Cuttings, Grafting, Budding) — Horticulture Grade 10", "Plant Propagation (Seeds, Cuttings, Grafting, Budding)", 4, new Guid("3c22945c-cc6c-6199-f614-746cb565217c") },
                    { new Guid("e8a695b9-34d4-aa36-b15c-9c53761e259c"), "Further Probability — Further Mathematics Grade 12", "Further Probability", 8, new Guid("c0eae66d-d3fd-8079-f377-be72f93a2d06") },
                    { new Guid("e937c530-f3e9-b2bc-5b2f-e507c0d28a53"), "Decentralisation and Devolution — Government Grade 11", "Decentralisation and Devolution", 10, new Guid("c5ad46a4-1f10-7aab-6b9d-9d32bca37e63") },
                    { new Guid("e9cfa1e3-3b65-a8b0-0f90-e2b57058c0f5"), "Computerised Accounting Systems — Financial Accounting Grade 12", "Computerised Accounting Systems", 8, new Guid("4d512f11-4373-8e2f-543c-0842a91f4cf2") },
                    { new Guid("eb484fb7-9df7-cf28-1e54-1e59d0285c2a"), "The Divided Kingdom and the Prophets — Christian Religious Studies Grade 10", "The Divided Kingdom and the Prophets", 7, new Guid("35b0b395-4f39-535e-d197-cb82f839fbbc") },
                    { new Guid("ec9c5067-023a-c0f0-179d-7fa4bf9985c7"), "Fish Health and Disease Management — Fisheries Grade 11", "Fish Health and Disease Management", 5, new Guid("77d36d32-edfb-63a5-b573-c9b18f58bb17") },
                    { new Guid("ecc339cf-18fc-16dc-0815-bbffda6d8a14"), "Public Enterprises and Privatisation — Commerce Grade 12", "Public Enterprises and Privatisation", 8, new Guid("6e0a7776-4921-0851-f05d-2a9cfe334aad") },
                    { new Guid("ed05a5d5-b177-f3e7-70b2-d7b15441b268"), "Fish Processing and Preservation (Smoking, Drying, Freezing) — Fisheries Grade 11", "Fish Processing and Preservation (Smoking, Drying, Freezing)", 7, new Guid("77d36d32-edfb-63a5-b573-c9b18f58bb17") },
                    { new Guid("ee3b2888-018f-2c4d-080e-624fc389cdfe"), "Globalisation and International Business — Commerce Grade 12", "Globalisation and International Business", 7, new Guid("6e0a7776-4921-0851-f05d-2a9cfe334aad") },
                    { new Guid("ef1132cc-accf-7cc9-daaf-83409e09f66d"), "Cash Book (Single, Double, Three-Column) — Financial Accounting Grade 10", "Cash Book (Single, Double, Three-Column)", 5, new Guid("cd368ced-9b4b-53c2-f818-261143f97c4b") },
                    { new Guid("ef59a5e0-e0a5-23d2-7431-f6c3a9a722eb"), "Computer Hardware (Input, Output, Storage Devices) — Computer Studies Grade 10", "Computer Hardware (Input, Output, Storage Devices)", 2, new Guid("43daec58-ceb4-fc4f-f1d4-086bd79d72a8") },
                    { new Guid("f051698c-c96e-53ab-2ec2-830bd42f57f9"), "Article, Report and Speech Writing — English Language Grade 12", "Article, Report and Speech Writing", 6, new Guid("9e7459ec-d351-be24-197d-c29757abab72") },
                    { new Guid("f087b81e-a088-2793-afc5-b6f8d7d248bd"), "Tourism and Hospitality — Commerce Grade 11", "Tourism and Hospitality", 5, new Guid("773db100-257e-8d22-3695-0e647cbb7fd4") },
                    { new Guid("f1dd0f8c-a5d4-4320-e8ef-3e5681e4279b"), "Economics of Horticultural Production — Horticulture Grade 12", "Economics of Horticultural Production", 7, new Guid("e868e7f9-be26-b7fe-6bff-f3ca7fd9b926") },
                    { new Guid("f20d7fa1-e33f-f0c4-6444-719b64f53d77"), "Purification and Salat (Prayer) — Islamic Religious Studies Grade 10", "Purification and Salat (Prayer)", 8, new Guid("598041c3-b226-5552-6f56-7b081f4d8279") },
                    { new Guid("f2a2dd94-ba5c-8cfe-6c66-b515013aec89"), "The Patriarchs (Abraham, Isaac, Jacob, Joseph) — Christian Religious Studies Grade 10", "The Patriarchs (Abraham, Isaac, Jacob, Joseph)", 2, new Guid("35b0b395-4f39-535e-d197-cb82f839fbbc") },
                    { new Guid("f2e3e8da-3553-8724-a370-7d3c89036743"), "Insurance (Principles, Types of Policies) — Commerce Grade 10", "Insurance (Principles, Types of Policies)", 7, new Guid("0ba6c666-e140-2220-8ec4-4f16bb2cd61b") },
                    { new Guid("f47eade6-1a2a-fe8b-d801-289772b92cdf"), "Christianity and Science — Christian Religious Studies Grade 12", "Christianity and Science", 8, new Guid("30f59780-4140-3f0a-cded-8b13fd7052cb") },
                    { new Guid("f4c23388-7beb-d4e9-3a5a-018b64ad3747"), "Islamic Criminal Law and Justice — Islamic Religious Studies Grade 12", "Islamic Criminal Law and Justice", 4, new Guid("0a0e509f-a99c-3193-c277-928d54224cc2") },
                    { new Guid("f4f3a322-050c-02fc-76bc-f8c1755e34fe"), "The Gambia: First and Second Republics — History Grade 12", "The Gambia: First and Second Republics", 2, new Guid("8c240df7-cbb8-b63b-c2fa-15ef2ff299d6") },
                    { new Guid("f619aff0-f183-ba86-0080-e4b417dc97fc"), "Selected Surahs and Their Meanings — Islamic Religious Studies Grade 10", "Selected Surahs and Their Meanings", 3, new Guid("598041c3-b226-5552-6f56-7b081f4d8279") },
                    { new Guid("f66f9a07-7c78-3936-e7de-2cd132240d8f"), "The Passion, Death and Resurrection of Jesus — Christian Religious Studies Grade 11", "The Passion, Death and Resurrection of Jesus", 4, new Guid("97ec5abf-b57f-0843-fe64-8f31af424c93") },
                    { new Guid("f67d9144-a0ae-7713-abbe-f3709d718cc1"), "Biotechnology in Fisheries — Fisheries Grade 12", "Biotechnology in Fisheries", 8, new Guid("aea3fc29-a3f7-76c0-8be7-f73335d78889") },
                    { new Guid("f69d2d22-ecd4-563b-95da-77dd7e59e00e"), "Trigonometry (Compound Angles, Equations) — Further Mathematics Grade 11", "Trigonometry (Compound Angles, Equations)", 7, new Guid("a115558a-224b-bd96-8793-629375a2869e") },
                    { new Guid("f6aea8eb-14ab-6068-0c84-b13b8ce73ccb"), "Warehousing and Storage — Commerce Grade 11", "Warehousing and Storage", 4, new Guid("773db100-257e-8d22-3695-0e647cbb7fd4") },
                    { new Guid("f7423f72-ae1d-d835-93ed-f9eb33caec04"), "Integration (Applications — Area, Volume) — Further Mathematics Grade 12", "Integration (Applications — Area, Volume)", 4, new Guid("c0eae66d-d3fd-8079-f377-be72f93a2d06") },
                    { new Guid("f7c36364-4a94-6fa0-2528-2782faac92a6"), "Economic Development and Planning — Economics Grade 12", "Economic Development and Planning", 3, new Guid("0d331ad8-0343-b862-4378-b5f017817a63") },
                    { new Guid("f7f4099d-c5dc-8f9d-1be1-fdd4b593acf2"), "Matrices (Operations, Inverse, Determinants) — Further Mathematics Grade 11", "Matrices (Operations, Inverse, Determinants)", 2, new Guid("a115558a-224b-bd96-8793-629375a2869e") },
                    { new Guid("fa6ce9b3-7a73-6097-8d42-c0221ad88456"), "The Quran and Science — Islamic Religious Studies Grade 11", "The Quran and Science", 4, new Guid("029f27a3-acc3-f8ab-b362-4a0d9c1db6f6") },
                    { new Guid("fac190f5-ef4c-71df-5f09-b4a7761a3bfa"), "Basic File Management — Computer Studies Grade 10", "Basic File Management", 10, new Guid("43daec58-ceb4-fc4f-f1d4-086bd79d72a8") },
                    { new Guid("fadbda6c-5241-f9ca-a888-39ce234bcaea"), "Sources and Methods of Studying History — History Grade 10", "Sources and Methods of Studying History", 1, new Guid("18c23abd-93c6-0e94-492c-dfd6d0a2ce28") },
                    { new Guid("fb86470a-6d02-a665-dd2d-f495c6926b59"), "Probability Distributions (Binomial, Poisson) — Further Mathematics Grade 11", "Probability Distributions (Binomial, Poisson)", 8, new Guid("a115558a-224b-bd96-8793-629375a2869e") },
                    { new Guid("fc3c35ce-fbc2-b003-0765-100a2c1fcfec"), "Social Issues (Poverty, Corruption, Drug Abuse) — Christian Religious Studies Grade 12", "Social Issues (Poverty, Corruption, Drug Abuse)", 5, new Guid("30f59780-4140-3f0a-cded-8b13fd7052cb") },
                    { new Guid("fd98d059-57d3-ad09-7302-1a93b6d8aacf"), "Early Civilisations (Egypt, Mali, Songhai) — History Grade 10", "Early Civilisations (Egypt, Mali, Songhai)", 2, new Guid("18c23abd-93c6-0e94-492c-dfd6d0a2ce28") },
                    { new Guid("fdf222bb-a4e6-5885-93d3-3a30bd66bd62"), "Nationalism in West Africa — History Grade 11", "Nationalism in West Africa", 4, new Guid("a33a76d9-ae1b-fb53-fef6-83cfc12992bb") },
                    { new Guid("fe12395d-4cf4-c610-8c37-a596a0e34fa2"), "WASSCE Past Questions Practice — Christian Religious Studies Grade 12", "WASSCE Past Questions Practice", 10, new Guid("30f59780-4140-3f0a-cded-8b13fd7052cb") },
                    { new Guid("fffedb69-67af-9b0d-ec81-241f8f2f3308"), "Feed Formulation for Fish — Fisheries Grade 11", "Feed Formulation for Fish", 4, new Guid("77d36d32-edfb-63a5-b573-c9b18f58bb17") }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("0042fff6-7eea-606b-73fa-3c4353582545"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("00988525-1016-8011-b62c-8d3d403a9de1"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("00a8e277-4667-1f0e-4897-991d0a4c4d03"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("0116c6ed-8307-dbf2-8b29-12c8e1a89930"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("0194dbbf-5c9e-ae66-bb6d-99a456e37faf"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("03c7938c-2c9a-ba3a-d4a1-17789fa4a4a7"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("040a6bb2-ac5d-1c7c-2272-f696027fde74"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("04371f1e-30fd-43f7-3a04-28df28a1e0a8"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("0446a6b9-f4d1-95d2-c483-55480dca9fbd"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("0451e2ef-ac0a-00ba-5d05-9c5aa92b1b9b"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("049f518d-2c42-97a2-07ec-c8fef7249909"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("052f877a-a0df-a8f3-a8c5-93c70d19324e"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("054d1791-1ad3-23c2-e615-b1eddb658629"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("0574f44a-0305-dd34-001b-b769cc27bfe4"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("073707a7-6f14-68f1-db49-6cb749dfa93b"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("076e9f62-e47c-b300-a3d1-d91a65210d69"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("079581b0-0fea-f36c-7275-48b44f8ed172"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("07b85e17-acfc-c650-6dae-f92579e11ed3"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("0854ecc8-a190-5282-e4dc-26febd613262"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("086fe156-0cbc-1ec3-922b-5649cedca154"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("08c49636-77de-23a1-404e-e1185aa9316f"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("08fd6d19-ad75-86f6-9474-8d5a85c74aff"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("0a4f38bd-1221-8ff4-840a-ad4fff3f0489"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("0ac76175-e7b0-3395-f338-cc6dfe970979"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("0ad004f1-2308-0ad0-b1b3-9ab0de48fa00"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("0c3635a2-378b-685e-2637-41059b33ac57"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("0cc1a321-2df8-fc3a-48de-59db7b61e3ee"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("0d86fa2f-9438-1bcf-24fc-20990b392592"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("0dabe316-73a4-3582-eaea-25e25b65e916"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("0ecd35ca-efec-22c2-da69-caed52eddde0"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("0f4ee199-edbc-07ae-96de-7f3b2c67ff26"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10a84cf3-d0c4-4817-badf-6be1d3c58684"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10c1d385-d2e6-a97c-0819-7a25f0530bad"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10f7e24c-5f61-2b8d-e6bb-44632c401301"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10fbfb84-01eb-a793-5ff0-b84fc359f916"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("112f321d-6396-88b6-dfe5-9401888beef9"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("11bf59f4-6cc4-6e88-627e-f34a2220bb90"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("12b1717e-7f07-27b0-2fe0-33e117531c53"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("138094d8-7dad-8c44-815f-5d6b4f47449f"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("13bf3a9c-0d22-a404-12ba-c1aa8a70a36a"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("13e0ac9a-4f6d-74d5-baf6-68c752bbe1b3"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("14530323-56fe-b18b-5c71-46a86b3fb4f1"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("1577a74d-ccf6-b5f7-b4ba-070a3a805554"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("1699dad2-619a-7eab-cb01-23576193dc97"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("16e946f7-d088-7b8b-d375-327268e65ea8"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("16ef2301-77be-2843-21c8-448971c86c22"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("176bb61b-e3b0-5d6a-f035-b6bb4a6cc315"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("17bab82d-baff-f5aa-d979-2c2c5fd6750c"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("18425af0-bd0b-8af4-4062-d187a96facc4"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("184f1291-f750-abfa-44c7-585fe1d7492c"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("195086fb-8882-c5a7-f88b-4e4a7812c59d"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("1a4387b4-01a3-9f06-9b3c-c59254654455"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("1a5fca97-770a-a0b6-63df-ede4d9e504db"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("1af1dc5d-81d4-24c1-146b-45f8ccf45020"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("1b255357-9b97-c844-146e-5cd377bcbc72"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("1b487f6f-f452-a92a-a090-e0348675faf6"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("1bfe485e-ba00-4225-5862-2d6deecf6eba"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("1c560b31-6e1f-ee40-3ae9-5e2c40dd0baf"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("1ca11756-83c2-4cc9-d653-60e0859bf279"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("1cedb1e4-eaea-bf02-5a36-95ee9aa36829"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("1d3c0e90-a4bd-3f3a-a005-543f4c69a357"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("1e1b925f-9c6b-c950-a5df-c098e2c84407"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("1eb2117f-2599-78cf-ebec-25cf8f46df3e"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("1eba05ad-34f5-d2e6-9f34-901d78c1d23a"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("1ec5aed5-874f-1f3a-0877-846256c15e7f"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("1f305f91-a2b2-8935-118e-279dbfd7f5cd"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("1fe44886-9d1a-7d79-cb86-938745207ab1"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("200136d9-0176-7a2b-785d-45b4410764e8"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("2044ce54-dae4-bf00-4652-f39a3721ab59"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("20db5bed-020d-3921-3ba5-8dfafb75a3a0"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("219ada12-2796-6ffd-116f-63c51722804a"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("21e47548-4432-8a92-b421-0840079acb1f"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("2243f8e8-7d5c-4b81-a791-270127b09134"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("224f067d-4317-ec91-4f93-e8020257fee7"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("227ff10a-f344-aa53-d75d-04a5421a4e7f"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("2309e4b6-b2f4-252f-bcce-0dbfecc65395"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("23c3f5cd-9642-361e-a165-060d54c7c90e"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("247ae126-cefe-56fd-0740-ee02fe42be6d"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("250bbf2e-b484-c6b5-6223-a3d34c6284cf"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("25f5af40-8199-8138-1ef2-e787ef46d6c1"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("262bea99-c2e9-960c-7dda-cd1524692bdc"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("267e1777-1c7b-8c36-2e7e-5bb6be197426"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("2682c933-8e04-9eb5-d750-a41580761767"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("268a9f60-6048-c9b7-af3d-5f75d2ea6d2f"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("290c1b1b-0c7f-b400-d727-54f2f28f677d"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("2a0f3bfc-6242-5f79-b948-6e4b8f0688c2"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("2ac38533-14d1-0022-aab8-d2dce5ff5678"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("2af0b667-4dae-b4e3-07d6-e35d7d8601b6"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("2afea495-91b0-b821-9d70-9a348bd0ecdf"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("2b1fe552-f577-f839-8810-6da3d5960204"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("2b6a59c7-c059-e196-201d-683fb17834f0"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("2b74fcc5-96ee-3d64-a903-2ae430f39447"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("2bf61e36-db03-9b38-49a5-9ec1cc8091ca"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("2c669eb7-be1e-0265-0747-b2a1f1d6aec2"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("2c6f9b0a-9389-4254-1b38-486bd405553b"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("2cb4b257-5efc-287e-c8ff-a697137bca70"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("2ccef70f-8543-4d6a-d6c1-f7d0b1620775"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("2d147903-ad40-92f7-a297-e02349fa6f8c"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("2d26137a-f0a3-f943-f2b9-e9116c28c34a"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("2d54d1c2-d0c2-6c25-b6db-3ca57b4d634c"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("2dd67d8f-4a28-daa2-03a0-a073f677dc38"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("2df790dc-9e24-927b-2853-ce14ed2ec462"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("2e6a92c5-a43d-4dbb-0ae1-d3224c004e31"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("2e742cb0-f9b9-39d6-a22d-aeb702353fb8"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("2fad72cf-9d7b-a3eb-6c75-a64d52804ebd"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("3129601a-0b01-f9a2-9e17-42cfb26cb39a"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("31dd619a-033f-5f35-ded2-0a82e653b8ac"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("321d7703-ff2b-7967-3a47-cda216f3f097"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("32f6bc54-3d3d-2f1c-eb17-c2b6e6c18e97"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("332fa2ac-cc6b-7a57-2e6b-6f7dea6ac3da"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("33e7b244-5ec8-e69b-2add-9bbe17229cec"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("345a783b-fab9-35bf-3a41-51ccdd345987"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("34e41a03-6676-f545-ddac-557c3464eeee"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("34fa58ba-38ed-8754-54af-23a9b536ef01"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("3521dda1-4206-1fa4-42b1-74bfb465bee9"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("35b2d6a2-9f9a-5c4b-9524-d4bb4011026d"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("35e86414-0fc9-3ef6-1131-fcf18b25cd63"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("36633574-50b2-eb6a-aa48-661184f0c770"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("366db323-4ac1-9fac-892e-9037819bb038"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("372aa319-6e6f-2765-fbe6-0c66e770b2b3"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("372c5f8f-3246-ef1f-af04-37cf9acc2fa0"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("3770dbf0-a610-2604-6b6f-2907e7001df3"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("385bd0f0-1235-33df-79b6-4a41b90499d6"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("39563711-23a9-f93f-17c0-06be2a82426b"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("395bf01f-437b-e274-a53c-1925d463165a"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("39e6eb24-3b59-861a-bdd6-1f82086d2292"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("3b3a9afd-2f77-34c9-4e78-8cf36fd336d7"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("3b46ce18-01f8-ca95-e113-2dd8475bb358"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("3c0a8007-b68a-1dab-b222-b48f657abd7e"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("3d98392f-cfb0-8774-01af-4de9eaa817d1"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("3dcd5dbb-56a4-8e14-be6e-d4477770b140"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("3dea0e34-b473-8f35-cd1a-142988b0c530"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("3f0cc97e-578d-a456-01c2-b952d42fad27"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("3f3e510d-4158-a222-8b3e-00e04ffc8594"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("414a97cd-1ced-8249-1cf8-c014ea1173c4"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("416b09a4-43f2-eeda-0177-34ded7211777"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("4217512d-4d37-34e6-d21b-78600bfd4ee0"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("424c681b-32f0-880b-d53d-c510d38c34b7"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("42e0298d-d193-bf8c-1006-5a401316ef44"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("434662b3-c252-6dd7-8de6-02a517fd0eaf"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("439c4bb9-3f75-22b8-20c3-f865d740ad79"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("43fec339-3100-7e54-870d-f8ec34bc0094"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("44b57ca8-bbe7-05db-6e76-f4a23cc572c3"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("44d12cfb-912a-070e-62ec-8b16aa7b40e0"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("4505ec5d-f9a6-cf61-ec8c-3be513694042"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("4527328c-43e4-cf41-4b78-c714e95d23ce"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("45306343-3793-48b2-1771-f25a88b64d3c"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("45cda1ef-c11c-ed3c-8251-189aa7b0896d"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("460efc92-a33f-64d7-ee2e-d3a8f8b5110d"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("46201024-bbf9-fa41-4343-5310f6be18b7"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("463f2576-8c43-df01-cb9e-30b3cd8c57ff"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("464f00e0-3028-f0d3-9e7a-2e76fa02f610"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("464fa3f4-7212-18d4-1829-66e8bd3438a0"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("4668fa90-9584-0cf3-3032-9fa4b6096dce"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("46921bd3-c7bb-d3c1-5b24-9f4bbb976c60"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("46c8a933-1e73-70be-1747-cfb495060260"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("47544b06-cc4c-846d-7dfc-1f4cfdc5dbbe"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("479df6a9-ef62-6ca0-4942-216cecd8fb0c"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("47a7ce5b-89e8-1026-7943-0b6b7ba94a74"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("47d24a92-000c-6bea-7e4e-ba0d4e8d9477"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("47f3bf90-47b1-f557-e8fc-fa715418f1c1"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("47fefc59-14de-0e36-2cac-2a13b73be938"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("481881b7-ff10-cee9-317e-adf55d0e1a64"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("4821a5eb-2b0b-f282-bb81-9f21ea6c4492"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("483742e5-d73e-1ab4-f6ac-eb8ffa2df637"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("4870d615-a236-4ba1-0341-3aa4e6c97310"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("4891705d-e248-9e45-d9dc-e21ae231afb8"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("48b17420-dd20-05ee-00d9-aff48057a2b6"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("49161299-aadb-c6ef-7d77-bf187211977b"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("49c91862-5372-ea78-1898-f2b00ae430d6"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("49ec01d9-fe64-5715-76ea-97e1f52fd8c1"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("4a6c6d9b-09bb-b459-2c16-87ac13af9061"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("4adcf74f-f687-d127-2e22-33154066f865"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("4b20b1d8-e819-c56a-2db3-a6403775fcae"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("4c658e9f-10ee-923a-9528-9c3c7809aca6"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("4cc1eede-c546-e911-e169-cbf75574c951"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("4ceb0762-621f-e1f6-e82d-717901ee7776"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("4d6ce23b-9743-4d32-a88e-960d9221d3d6"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("4de7d120-a521-a277-4c45-6cc6c9cb9a8e"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("4e488e6e-a6ca-a411-ce6a-1e33b6986d16"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("5047f972-a25d-e62d-19fd-c2ea17703aae"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("51994855-d2f8-098c-efd3-5da7fee70cdc"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("51abd258-81a5-c246-db02-c075434fbe4b"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("536cd1a5-10e6-ebc9-2da0-7952b1455dcd"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("53c6db7e-3a53-5218-a7e3-ce08a39ddf94"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("5423176d-4cf0-689f-4bff-f8dd708c8cc1"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("545e7a46-de5c-9410-03c4-f9016ec4b911"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("5469ee84-ff7b-25a1-9997-8a33a1efe159"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("5522b60e-5ae9-35bd-d1b9-6d12e8565de1"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("559cbb9a-8a85-d896-3f43-d2124539a2f2"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("57103e21-91f1-cc4c-1fa9-daef5de110f3"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("5729a2f3-212a-cd84-db61-c902de27cdab"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("581db8be-6237-041a-ebe4-f8022928cb2d"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("582aadc0-afc6-f2e4-249b-c884c3d410aa"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("58e6231b-500e-19ef-edc7-4d935d06a3eb"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("58f2e1f2-11a8-a9fd-f23c-a85662e20f7c"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("59370825-361b-f3bb-3c5f-9a7fbb95b33f"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("59c82afd-e635-b5a0-a637-45a7a300e6e4"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("59f2429b-0cff-f3fc-400d-8fa5db4ec908"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("59f496db-8cda-b0bb-479b-999a22c66b4a"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("5b0688b4-d23a-e306-f7e4-5ce33c55c07e"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("5b4fa2e4-a927-595a-d10b-e59c07993e41"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("5cb614e7-ea6a-1436-ac74-ad4915253d2a"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("5dcad4b1-e035-e6b1-20b3-d5d5e126cfff"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("5df77b2f-c443-270a-2563-889aa434c8ba"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("5e26de40-0c9e-15c8-106f-72ba66d74dc8"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("5e2b35b2-46d4-6907-b8af-ed06ada31e20"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("5ee786fb-0ca2-4ac2-11f7-98004763d7a0"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("5f8383bc-e3bd-0f9e-cf47-dc1402920124"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("5f89fd25-22fa-cc32-0f2c-cf4a2721e385"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("5fba14d0-46b3-144e-8c42-bd68bba1685b"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("5fc4e27b-dbe0-39fc-d9df-634686f2fa0e"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("5fcb450c-7553-d7ce-e96d-35cc0c6986fa"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("60715326-cfc0-637d-9ae7-1c8dfe0f00c0"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("618ce8ef-36b1-b7bc-3100-7a0eaaaefdc6"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("622e7382-a000-1393-8271-31c82f17a945"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("62639995-6184-826c-bdbf-8280a73efe59"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("6294c248-a183-2fa7-2ef4-0aeed9f9a3a7"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("62dc0061-a6f6-dcae-8c5d-c4fd34179834"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("64499a27-509c-af71-2494-c63c86626a07"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("65287e2c-455f-ea29-7cf0-3add99ecc655"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("6570128f-2119-c14f-7b5b-80db9bb48d52"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("65761251-76e4-78ef-3af9-335cb5830426"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("659a4f4c-3338-d8e8-672d-cd63af50be90"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("6635fedf-60e5-331f-ea6d-72bcfde958a5"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("666852fb-a03a-5d58-420a-0a229adc19d5"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("67124016-1d49-4d91-ed31-dcf88116f421"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("68305741-a8de-c198-1e14-77bef1cdb44d"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("683e25b0-54eb-916b-40c7-9100f226e0a6"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("68c0a229-7cc2-494a-1797-de74b5842cc6"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("6959e07f-d270-8a51-f451-a0a391a3d9c2"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("6aaa7896-d28d-6cb4-9bca-59c9429adad5"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("6b13a098-94be-156a-f1d6-72bb14847c8d"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("6cc29ab5-b2a6-39fb-17a7-a502182ae878"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("6d3dccc9-dad4-46e4-874e-ed9edb68c8b4"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("6d79663c-eef9-f8de-c939-1cd1b5430436"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("6e8bfc6c-36de-e700-9c5d-0f54ac2898ab"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("6ea4d355-143a-bbcc-8cca-988a367b8408"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("6ef85e32-2dc3-fb77-8ee9-5f20841e8dfd"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("6f104f94-0099-d7fd-cefa-199df5e4f701"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("6f6059b3-4563-9016-f01a-fe7e9ef0cc3c"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("6f967ce0-da0e-21c2-518e-6f0848d13f47"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("6ffa6dc9-7365-1355-df29-ca8ba4304116"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("7086bc64-a522-d370-3a01-70d9d1332870"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("70c74db6-d260-d61e-5e0d-d54473f56337"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("7144fade-f89d-3858-3647-7488cd5ff5dc"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("71f45318-fdf1-7ef6-b431-8cf4c229153f"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("7262fa46-2516-9699-26af-95ca78c6a406"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("72aac255-9983-0af8-616d-b6bf9b3355bc"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("73858aee-88d1-b56d-3e4a-1463560bbb64"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("73b82679-102c-6f94-21c1-344f103c55e4"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("73ce0e90-3030-f5eb-96e0-b42fd27028bd"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("75077c02-416b-3a66-95e2-960f71df1cf5"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("756abca4-8d6a-678a-21dd-ea4d60f17652"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("759b08fa-3223-d872-2003-73daca8ee247"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("75b8b1da-49bb-a882-5e6f-a8bd0ceedb79"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("75e57399-2f30-ca66-e270-b37110e9a00d"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("7605e15d-03ea-430f-e766-20487a1b9646"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("762de910-7db7-2920-5a30-9b0d2e4a7023"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("7633dea0-d21b-2214-727b-c10e1d498b73"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("769fa036-f03f-eaf2-fe00-7cf850bdbf4c"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("76b93960-e64a-8597-604a-ad92b0bf4dfc"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("77875edc-f504-eeea-afbf-bdea83ff28d6"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("780ad161-7c59-b149-9bdb-dfc5b34ec40e"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("78b8fe0b-da9f-797b-f8ce-1983ad7204ac"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("78de959e-0333-6bd8-cc4f-2dd4d1ca71ce"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("79692199-9913-c00b-1422-24f79954003e"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("7a1ab915-e81a-6bf4-c42b-d0fbb53c3e8b"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("7a21a118-3382-fd30-e462-1dd6d404f58a"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("7c3016d9-09b6-7848-d994-38b0e6a91d79"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("7c59897f-3065-8224-9c30-90f3045cc12d"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("7c8a5da9-2a50-aa33-0168-ad1b2ed25967"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("7cb68f96-3b32-8ddb-28bd-89abd81d1add"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("7cc21cb8-e89a-9713-274a-7bed98bce15a"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("7cde5f2d-9c81-6479-be61-e33e495d7ace"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("7d0a65e0-92dd-e83d-1c2a-7cca95191241"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("7e1d6e0b-a6fd-0585-daec-6fee40be7c27"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("7f21b63d-70c7-c26f-27cd-f689cf7a2e90"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("7fa3504e-0b0d-298e-1c01-859202a6b422"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("814810b4-f8d2-85d6-90ee-fa5e3136f220"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("8189ad90-b599-d14b-d71f-c5af5846b535"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("8194a10d-fe45-7994-3a1c-552f9b37b7ba"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("81a24229-7e92-9ae6-a61d-c3bdff2f3df6"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("81d96f62-d8c5-33bc-f8f2-68936519bde5"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("82e11fb2-ec88-257f-f40e-5a3fa7dc4ebf"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("831f435a-ee19-aa98-74b6-dc983aefc71c"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("839f6aa9-3188-83b6-2ae7-f038ff888a28"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("83eef78b-f763-c601-e5d9-b923459cbd07"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("8457c068-bdf2-d993-e55b-3bef3ebfdae2"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("8495506f-1f7f-a2eb-ef3e-c81708fef232"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("84ab693e-8a82-c265-ed22-13fa4e91211f"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("85072438-b10b-4b0c-5c46-f01237bd64b9"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("860f89aa-b3a7-dc15-ffe9-fdcd7d2f9595"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("8715610a-99df-d8ac-f78d-18917b54895b"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("87665af0-d4ef-0687-63c4-4043b3cdd516"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("87c41ba9-26bc-a632-76e3-39f1845966ba"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("88c2627c-9bc9-f1a7-046d-f8785bb3d537"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("895ced0e-9390-528f-f2b3-213a050955e5"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("89de3736-f73e-15fb-2a12-f88f3e1aa006"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("8a056353-85cc-4012-36ac-edea89a35dd2"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("8a38e09f-52b3-da07-3601-5f29bccfa6ee"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("8b666b2c-5e8e-a0c4-17c2-8b6a9ffc67cb"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("8c009404-b43f-6103-4cf8-3fa516251337"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("8c0221e3-b2fa-de12-daf8-d94fc88082bd"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("8c53306e-2816-61a1-87d7-b77ba83bff14"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("8c5ffbcb-913b-da9b-9f2b-6b82d8d29230"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("8c96db37-f2ed-5000-f07e-ba8120e2e73a"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("8ccff01a-a58a-eca0-607a-af7afa55ceb2"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("8d914fd0-c570-35bf-b699-ab0469471992"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("8d919dc1-b69c-c1df-b4e1-c1cdb6e1cb93"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("8da30564-2e73-adc8-3d41-33646d06632f"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("8dc572e0-60d8-6190-78b1-10872bd512fe"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("8dc98e0c-1145-2d51-5871-c4ba49f97d95"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("8e62aaf4-eac6-7240-e63e-9dca9775ad9f"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("8e7863c6-9496-baae-1184-be68e2c2e7f5"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("8ee08337-5195-8a47-c4c9-a5f126ce6b2a"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("90996712-c7c1-4dea-0d28-836eab346490"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("90a962b4-89f3-9513-701c-591adf448cc2"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("90cb99a1-80a3-f69e-36bc-a0e4db229973"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("9198b903-0ae3-9408-6761-e898208d6df6"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("92b0f620-8a11-9591-ed1c-54fd9d9e9262"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("93fc90dc-2ae3-fa31-1bb2-7a98f6a86e89"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("947dae29-0291-7252-b08c-d0db61a9ed94"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("962beebd-0018-45fb-27e5-b9e26907b876"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("9656578b-7068-fd94-1ce4-d46cd73b4177"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("96724fb6-a875-66d8-9360-63c21e88f34d"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("96d8ec76-9859-2e70-76a7-dd83cb195745"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("9862fe5f-522d-24dd-ca89-ada7e91f9bba"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("9871ca45-cf53-3d18-3cd9-f558d029a1c2"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("98870757-a079-6964-80c2-495781f40570"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("992316f5-4339-b9c7-3785-f4dfeae92c31"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("999b9136-8eb9-3918-cac3-c5d2e12738be"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("9cbe385c-c375-590f-fcb0-6248c01b0a67"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("9d33aa52-aa70-5ff4-13ac-b08642f0871d"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("9d6759f8-0de0-f6e8-3d29-dd837aee70d2"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("9d999069-8b08-4546-ab87-9276d8b83496"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("9e05d904-b4c8-8c19-a32d-3664443b5d05"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("9eba9460-c2dc-ed90-c223-908912166a05"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("9ee931fe-12bb-d6bb-8701-90fed351a761"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("9ef2f6f9-331d-3b10-8f59-f03329a9e76e"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("9f5aff00-a15b-85bb-d775-a3f8c46ed2b4"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("9fa153e2-0380-6e52-7478-b567e580e218"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("9fe7f7a2-59dd-52c3-4a6a-e0eb1e78db50"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("a04550cd-cbaf-ef14-8333-0b5ae28557dd"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("a0b59b38-0b24-6e0e-8bc0-90a44457c6c8"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("a0d9ba4e-6165-1d95-981a-cccc316a8b14"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("a4ce9914-e83c-3326-7ce6-a93212e6379b"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("a4d05415-560c-c6ba-9ff6-0652e55c71f7"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("a502c8bf-0c86-7ce7-346f-2c5f86c37502"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("a50e398c-9073-f304-4f07-1d9051d757a9"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("a58ec9a5-80b4-6000-6934-5490d702ed98"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("a593e80a-cdb9-08f0-9452-0b58678067fe"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("a5e87ac5-7183-3cc2-6883-ecc75da8df01"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("a5ea5a3d-3c93-57dd-40f8-f5d81595866c"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("a635fc05-f3bc-9bd3-1fe3-423d3c3a19bb"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("a6406211-28c3-2103-9e94-fecf6bd49102"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("a6e1aa10-686e-c1cc-3f3a-76baca6bb892"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("a70616d7-4a38-b49b-84fe-591f1a1a3d49"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("a77e69bd-7999-b09a-892f-433925bda0f3"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("a7c1171f-8023-49c7-4a20-a9e3988986db"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("a7c91f27-5cc6-8de5-85f9-5eb83eeb52c6"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("a7d4d5d0-afcd-5f7f-c2c7-d0d5891073a3"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("a839cfde-ad9c-4754-4e18-f52be917be2f"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("a86df0b0-1f48-0c48-8452-c23269cd9e6d"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("a90fb801-483f-a6cc-d597-df04e56ad6c5"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("aa0d88ac-a24c-057f-7554-80c39d2b306b"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("aa171a3e-aca0-17be-50e2-21c3e5c41522"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("aaec6b71-a067-cb18-27df-4540268e4e81"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("ab8f974c-a503-1789-4d35-8f6d8b6be739"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("abd3382e-ecd2-3a0a-7898-4190e752efcb"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("abfea02b-7c76-77f7-f39c-c58a20db6fee"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("ac4cf89c-49aa-ecf4-27c1-95a27c1cd0d6"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("ad544a15-a25d-1735-8c18-bbd644cf4386"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("ad65bfcc-86c2-e95e-3ec7-a835fa0b03a7"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("ada8d8a9-46a2-4fac-2b4c-79c6400f668c"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("adcc06f6-0c6c-28b3-fd04-733aafa6883d"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("aebc62aa-74ee-ce2d-9be6-53db787f76aa"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("af405fbd-33bc-f9bf-9d33-99eb3f265837"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("b0064c3e-37d1-c177-b47e-06643aa167ae"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("b00fa90c-01d3-c068-aa42-6b547e58fc77"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("b0a7c71a-711c-9ef0-6be6-6ef20b546a26"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("b0bd56d8-19b3-e4ce-9f22-54127f3452c6"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("b0cae5e7-4a34-5c81-5e53-949dd3e3f7ec"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("b0f7123e-5997-26c6-bd4e-ac1806213727"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("b14ad301-413e-6eff-ebb8-38f9b2a97c0d"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("b19a1355-f187-06b0-cc62-901e7b216097"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("b1d7d357-02e3-ee1a-5ca8-16818c591fff"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("b27072b9-04b8-446e-53c1-29b19c3de8ea"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("b5048787-a638-c63b-3c4f-0388e11ce8d5"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("b50eeeff-b0ad-c1ec-1920-b45731f69137"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("b5a068e9-228d-6c5a-cf32-5c3bb1d25eab"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("b645e12e-4305-ced3-f9aa-1aca899b16c1"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("b76c3ec8-eb6a-6ab6-f3a4-c9ada0006b09"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("b8dc7f3f-024a-4b89-8877-64fb05930ef2"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("b9422bef-1ece-44f4-27bf-480b5a0377c5"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("b9ecd87c-9681-9a46-6086-2115b1d6c3a7"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("ba8edbf2-73b4-1b86-a489-41bdf1ffc477"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("babb95d7-5db9-2f96-e1d6-8009ccbf15ab"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("bb439a1b-1ed7-ccdb-aedf-3e38c71d67a5"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("bb54aaea-eb49-042c-8bf7-46f2155d2adb"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("bb6f0378-ce5e-32db-e0c9-a89b51413ab7"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("bcf81166-e945-7d8b-fb42-18efbfedf1c1"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("bd8072f1-fd0a-4656-db4c-eeea4c88eb6e"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("bdb5b4b9-db40-b972-0ef7-474cc9e5f65c"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("bdbfc482-07ad-6a18-4842-87452cd599c1"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("bde1c280-6b64-aacf-70f5-aa35a10fe219"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("be1c942c-d410-b07c-c413-123a78d1676e"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("be605b33-0e65-58db-9b7d-335a37cb5d9b"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("be6e0366-42c6-8cd0-9791-4d4469a17540"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("be8b17a0-aaa6-5d2e-4ba0-3ecd2d6c8237"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("bead5d43-7846-3e45-5b85-6e832d7dfb32"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("bec7edf6-fc77-d145-920b-9ab648e784b8"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("bec8a9d6-0d6c-9fb6-5dec-4e8ec8be31bf"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("bed34e7e-dd98-3b56-5f90-b21e3fdc40c6"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("bee7a0d1-3f25-2a8e-01fc-357a8352a0c2"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("bf08b4e6-b503-e717-9652-bb656a48782e"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("bf587877-8deb-da73-92b2-0ec60d694764"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("bf5ea608-8c78-902f-a073-f5882366c019"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("bfe4c3f0-a695-5bee-0ca5-c709fc62e845"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("c06ff719-754b-d6d5-f4ed-da66924fb992"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("c085ba58-1b9d-b425-aa3d-d8183270ce74"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("c1976dc6-d9c7-b3bd-173b-d408b4b0b268"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("c226ff6d-8b3e-a606-63d4-c61c9fea890c"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("c34bb655-bee4-eedf-7356-a6fb788c169a"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("c3fb7d1b-108c-4101-8060-4f84fbd6a81c"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("c43e735d-dbd7-5598-40ef-06f45fe7ccbe"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("c43e963a-abae-969f-698b-808daf65ba87"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("c44a7729-fe7f-6ce7-9adf-236ea00d79db"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("c48e55eb-c3f4-05df-6b62-f5c59a950ac0"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("c48f92bf-ec87-8a06-6a13-027bd483ec17"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("c4ed813a-958d-2f3c-0d62-1119ede89ffd"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("c5389e79-0f83-f835-ba36-4dfb4b637790"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("c606033e-b2dc-67f4-fd5e-a602b8f09d85"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("c65e15be-8b0c-51a7-fad2-c165f23ce233"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("c672d2e8-fae8-9eed-3a44-ea5fbf0de465"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("c689bc6e-80c3-6346-8bba-02665e1ac8dd"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("c68bcacf-7906-de5c-c7f3-c1d5b0e90ba5"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("c6ab93ca-93b2-c527-74ed-57f57b363f89"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("c6cefad6-c101-dcf5-25d3-cdcc69cfde5b"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("c709ac91-b15e-39df-45e9-0ccafeb9ee8f"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("c75edf5b-0f18-35b6-08ca-4912b198d66d"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("c7af1d43-6649-54aa-bac8-91d91d828f8e"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("c844ee5f-24c8-9929-8254-27eb9d18f2aa"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("c8d00f57-21f4-1e33-0f09-98b78a5e4c65"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("c95f3651-a0c5-94e7-fd59-8363838ccbf3"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("ca5bb240-ff3f-8fd7-82fa-db86f34ac516"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("ca8c5c8a-5b3a-3b3b-28a5-1c3e651a7c98"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("cbaaa277-dba2-648d-f113-59fb0fa6ca42"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("cc31bbb7-6e90-b1e9-3316-c622caee97fd"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("cca4f5f7-2531-6503-4ee1-6f854789bf65"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("ccd9ccc6-a02b-0ae1-9b8e-1217fd227ec7"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("cd11e8e6-1d73-998c-9801-d39ec3ad537b"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("cd37d1c0-4098-944a-0d2b-243277c2b52a"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("cd6813fd-d843-35c4-e1a3-cbb03e5a3bed"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("cdeb60b9-6582-3e3b-b9a7-b8e432ac84b5"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("cdfe0db6-66ba-42f0-baef-e35200f8e3e3"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("ce2ead18-c9f3-72be-5d36-d46b15699b80"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("cfb2cb53-4481-3dd4-a97b-35daece3b89b"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("cfbf6b22-f2f6-dd90-9c68-0f00c2a432d7"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("d078acd6-71c5-bac9-9e1d-59508f845a5e"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("d0a9e4f2-e994-203b-4b0a-02a17d3d9e4d"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("d12e24f4-addf-493c-bf24-ed72b3179eb9"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("d33c69f4-4f9f-b665-4466-933f01d264e1"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("d36954d4-150f-419e-89f7-2d4c69e745e0"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("d376a968-f982-3fc3-ca70-2e6c670ccd19"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("d3ebf823-652b-837f-cccb-c0dcec37942c"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("d58209ae-5619-7cb6-8a15-25db24f7f6e8"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("d5e88fcd-5628-845a-6cd3-817582c02f23"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("d63c256c-f0c2-d71a-9d8e-8d70ecdfa3b4"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("d687c787-b771-df5f-ed0e-31154b09649d"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("d6f0d4cb-ebca-9957-0e76-5e77799ac336"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("d7ea2f0c-a109-d8c4-7ba0-ec13add19fe0"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("d80bbf4d-a28d-5ae7-b780-bfee2d03ef31"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("d815426d-017e-c103-b345-771e519fbf62"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("d853e542-d37f-35ec-6637-f0fd3cd18946"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("d86a8dbe-6b19-80b9-033f-feb4f48159ca"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("d8f0b4c8-b43e-c0d7-0b51-d7c462430014"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("da6e9201-055a-dba8-12ad-fd85745dba81"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("da8d12d4-acbc-8fe1-68c7-f60adb34a66d"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("db7be8f9-9d77-06c2-61e7-4a529afa984d"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("dbcb578c-5ceb-fa42-e3f9-5e8fa1ba5fbb"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("dc54127c-9220-0036-58b9-6e27b0ab7a07"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("dc6f8665-ef31-7ee0-0282-3e86812f7f37"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("dcd2c783-c8c7-6bbc-8a3e-edbca1cb89eb"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("dcd81206-f567-43f8-2d58-62ce548cee37"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("dd6fcfc9-5873-af97-4a8f-f69f51265ae7"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("dd8a8eb2-560b-b1a4-3df3-af9df6344dc3"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("ddcafbfe-400b-3369-ae82-5679a11bea6f"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("de185619-221c-bc79-a90a-4f11ecc89c84"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("de647112-e4fc-1469-3e2f-766f1b8c47a5"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("df731687-b4cc-2267-9639-d3bcd8461547"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("df81ed8e-bc16-2bcb-c53e-b1b686ac96a6"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("df9980c0-681c-d8be-682a-9b3839ef9298"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("dfba7a2a-5c14-c802-17f5-7cfc0addff5b"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("dfc4f7bc-f0dc-7dd0-392e-742da60583a4"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("e00193f5-2dd9-b11c-4a04-cfe494939732"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("e13ba512-5382-7fb9-cf1b-13c5007692ba"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("e15ea2f8-eac6-d915-2ec2-d23c81666150"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("e17d9891-fd78-58bf-4cd9-6653532f15b9"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("e17f6d51-92b7-9d6a-c9e9-85e3564c477a"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("e1e58c26-b9d7-94cd-71f6-f1dce6da150b"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("e1f51036-cbfc-9015-a3da-94c549f1e969"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("e1fe9dfd-3ce5-a6fc-ddd1-1118d8c0ea20"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("e294defc-e9e3-090f-54cc-e5708481d7fe"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("e2fb33cd-7480-6439-ae51-4726a81c3520"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("e3687575-b413-7d46-9893-2da6ab44671c"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("e36ba8e2-613c-c2ba-cc72-f470267cd53c"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("e428783e-6ef7-9528-df6e-4ff76b0bbb37"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("e4b60c45-6fa9-1111-744d-e193533565db"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("e63d564a-4a60-6673-d095-154feca8078e"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("e68e52d4-4a80-6c8f-ed5f-a6100fc69453"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("e6b2e006-d050-cf51-e3dc-1835a18ab8ff"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("e6e67b46-ebb3-9e9b-00e4-397ed193a1c8"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("e749f9cc-b976-e972-5a4e-0517551e75a4"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("e8a695b9-34d4-aa36-b15c-9c53761e259c"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("e937c530-f3e9-b2bc-5b2f-e507c0d28a53"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("e98af30f-5ffa-b438-5f86-bccfd6f21899"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("e9c21c4b-507d-b91a-cd0a-69ccbe6246b2"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("e9cfa1e3-3b65-a8b0-0f90-e2b57058c0f5"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("ea16fa76-8f98-d05f-126b-fb1836257b54"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("eb484fb7-9df7-cf28-1e54-1e59d0285c2a"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("eb582ba4-a9a5-1504-96e9-ac95e265b1f9"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("eb7878d3-2091-e755-462e-0f9c2a349865"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("ec9c5067-023a-c0f0-179d-7fa4bf9985c7"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("ecc339cf-18fc-16dc-0815-bbffda6d8a14"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("ed05a5d5-b177-f3e7-70b2-d7b15441b268"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("ee3b2888-018f-2c4d-080e-624fc389cdfe"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("ef1132cc-accf-7cc9-daaf-83409e09f66d"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("ef59a5e0-e0a5-23d2-7431-f6c3a9a722eb"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("eff9b4f3-c029-d06b-3808-479be99fb66e"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("f051698c-c96e-53ab-2ec2-830bd42f57f9"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("f087b81e-a088-2793-afc5-b6f8d7d248bd"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("f08994af-5f8b-cf6e-9e0c-3149bce9c19b"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("f1dd0f8c-a5d4-4320-e8ef-3e5681e4279b"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("f20d7fa1-e33f-f0c4-6444-719b64f53d77"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("f2a2dd94-ba5c-8cfe-6c66-b515013aec89"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("f2e3e8da-3553-8724-a370-7d3c89036743"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("f2e774dc-376b-2d70-0959-7951d08caad3"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("f3260204-f771-3cdb-67cc-18f16b4941e8"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("f47eade6-1a2a-fe8b-d801-289772b92cdf"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("f4c23388-7beb-d4e9-3a5a-018b64ad3747"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("f4f3a322-050c-02fc-76bc-f8c1755e34fe"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("f619aff0-f183-ba86-0080-e4b417dc97fc"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("f65eb5a5-7fcb-794d-ea27-074d6eabd45d"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("f66f9a07-7c78-3936-e7de-2cd132240d8f"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("f67d9144-a0ae-7713-abbe-f3709d718cc1"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("f69d2d22-ecd4-563b-95da-77dd7e59e00e"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("f6aea8eb-14ab-6068-0c84-b13b8ce73ccb"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("f6e7e4ad-5fe7-b9f2-8d73-d8bbeb918287"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("f71d5405-b478-a735-bb9f-27119c097bcd"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("f7423f72-ae1d-d835-93ed-f9eb33caec04"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("f77b8d01-6e58-b0cc-af73-720561a5b83a"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("f77dfc64-755e-b4db-85e3-b0962ec5edd3"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("f7b67f08-0cb4-8bda-52ef-ddb4919cd95b"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("f7c36364-4a94-6fa0-2528-2782faac92a6"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("f7f4099d-c5dc-8f9d-1be1-fdd4b593acf2"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("f7ff93c4-c977-dd0d-17fe-f025e9654c13"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("fa6ce9b3-7a73-6097-8d42-c0221ad88456"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("fac190f5-ef4c-71df-5f09-b4a7761a3bfa"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("fadbda6c-5241-f9ca-a888-39ce234bcaea"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("fb2f36ee-8b47-9219-8cf7-4000eb32a1b3"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("fb86470a-6d02-a665-dd2d-f495c6926b59"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("fc0ce933-eb82-02be-d378-aacec0fc3963"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("fc3c35ce-fbc2-b003-0765-100a2c1fcfec"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("fd3dbd5b-b789-7cb4-23f6-bd9fd9c3e828"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("fd98d059-57d3-ad09-7302-1a93b6d8aacf"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("fdf222bb-a4e6-5885-93d3-3a30bd66bd62"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("fdff7b44-f5d8-d9de-a9fc-d54ff383c2e6"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("fe12395d-4cf4-c610-8c37-a596a0e34fa2"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("fffedb69-67af-9b0d-ec81-241f8f2f3308"));

            migrationBuilder.DeleteData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("029f27a3-acc3-f8ab-b362-4a0d9c1db6f6"));

            migrationBuilder.DeleteData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("04fbf1c3-6b48-d6c9-e60b-8af777b7fa50"));

            migrationBuilder.DeleteData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("0a0e509f-a99c-3193-c277-928d54224cc2"));

            migrationBuilder.DeleteData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("0ba6c666-e140-2220-8ec4-4f16bb2cd61b"));

            migrationBuilder.DeleteData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("0d331ad8-0343-b862-4378-b5f017817a63"));

            migrationBuilder.DeleteData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("18c23abd-93c6-0e94-492c-dfd6d0a2ce28"));

            migrationBuilder.DeleteData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("30f1307e-bbbc-d420-61c7-124673a9fd1c"));

            migrationBuilder.DeleteData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("30f59780-4140-3f0a-cded-8b13fd7052cb"));

            migrationBuilder.DeleteData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("35b0b395-4f39-535e-d197-cb82f839fbbc"));

            migrationBuilder.DeleteData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("3c22945c-cc6c-6199-f614-746cb565217c"));

            migrationBuilder.DeleteData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("43daec58-ceb4-fc4f-f1d4-086bd79d72a8"));

            migrationBuilder.DeleteData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("49841f61-c212-8eec-2810-f1bb824e37d2"));

            migrationBuilder.DeleteData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("4b421fd5-92e6-ef02-edd7-7804412bedd0"));

            migrationBuilder.DeleteData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("4d512f11-4373-8e2f-543c-0842a91f4cf2"));

            migrationBuilder.DeleteData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("598041c3-b226-5552-6f56-7b081f4d8279"));

            migrationBuilder.DeleteData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("661ec45c-8411-3167-8af8-544152cebce6"));

            migrationBuilder.DeleteData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("6e0a7776-4921-0851-f05d-2a9cfe334aad"));

            migrationBuilder.DeleteData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("70a66f3c-d5f9-1b35-bc9c-119ef3d85f3e"));

            migrationBuilder.DeleteData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("773db100-257e-8d22-3695-0e647cbb7fd4"));

            migrationBuilder.DeleteData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("77d36d32-edfb-63a5-b573-c9b18f58bb17"));

            migrationBuilder.DeleteData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("8c240df7-cbb8-b63b-c2fa-15ef2ff299d6"));

            migrationBuilder.DeleteData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("94f51606-1b58-9322-ecb2-cc8be90adce0"));

            migrationBuilder.DeleteData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("97ec5abf-b57f-0843-fe64-8f31af424c93"));

            migrationBuilder.DeleteData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("9e7459ec-d351-be24-197d-c29757abab72"));

            migrationBuilder.DeleteData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("a115558a-224b-bd96-8793-629375a2869e"));

            migrationBuilder.DeleteData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("a33a76d9-ae1b-fb53-fef6-83cfc12992bb"));

            migrationBuilder.DeleteData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("a4eaeeb3-b7a3-9abd-1c53-5309c8d473c0"));

            migrationBuilder.DeleteData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("aea3fc29-a3f7-76c0-8be7-f73335d78889"));

            migrationBuilder.DeleteData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("c0e3b77a-b7e6-f84c-1414-a9b4986364c7"));

            migrationBuilder.DeleteData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("c0eae66d-d3fd-8079-f377-be72f93a2d06"));

            migrationBuilder.DeleteData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("c33eef6f-8e20-9183-f5a2-4e5187de42d7"));

            migrationBuilder.DeleteData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("c5ad46a4-1f10-7aab-6b9d-9d32bca37e63"));

            migrationBuilder.DeleteData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("cd368ced-9b4b-53c2-f818-261143f97c4b"));

            migrationBuilder.DeleteData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("def2214a-a301-249e-c656-0914b4ecb5a5"));

            migrationBuilder.DeleteData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("e868e7f9-be26-b7fe-6bff-f3ca7fd9b926"));

            migrationBuilder.DeleteData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("efa84916-91b0-0dea-73a8-860fc9a80941"));

            migrationBuilder.DeleteData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("f081fc93-cdb6-c836-9743-149f52228aab"));

            migrationBuilder.DeleteData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("f1ddfb49-3ee7-d0d0-3bdb-ba71e5d13aec"));

            migrationBuilder.DeleteData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("fffdd2fc-d6d3-7308-9d25-267ff832fd7f"));

            migrationBuilder.UpdateData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("06216eaa-a8e4-6710-e535-da8f3f9af209"),
                column: "Description",
                value: "Biology for Grade 12");

            migrationBuilder.UpdateData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("26a00ceb-e8ed-7c0b-b245-a60b6107d495"),
                column: "Description",
                value: "Chemistry for Grade 12");

            migrationBuilder.UpdateData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("45c2e4c9-1dad-eb06-8108-5a7b253db61d"),
                column: "Description",
                value: "Chemistry for Grade 10");

            migrationBuilder.UpdateData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("5295e6aa-8238-0794-a9ec-29deb0bef2aa"),
                column: "Description",
                value: "Mathematics for Grade 11");

            migrationBuilder.UpdateData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("629825b8-c2d2-7aee-d163-0be88495e272"),
                column: "Description",
                value: "Geography for Grade 11");

            migrationBuilder.UpdateData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("65ac38a6-5a9f-22e8-0de0-2181565e0d5e"),
                column: "Description",
                value: "Physics for Grade 12");

            migrationBuilder.UpdateData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("69e6f790-ae65-2e87-f351-982b99d92bfe"),
                column: "Description",
                value: "Agriculture for Grade 12");

            migrationBuilder.UpdateData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("6aaf47ef-e5a0-d4af-70db-f2011158ef8e"),
                column: "Description",
                value: "Biology for Grade 10");

            migrationBuilder.UpdateData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("76324b9e-5fc2-7160-7522-932866c0a77a"),
                column: "Description",
                value: "Agriculture for Grade 11");

            migrationBuilder.UpdateData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("7a475323-7df5-f613-6449-fcfad4461d7c"),
                column: "Description",
                value: "Geography for Grade 12");

            migrationBuilder.UpdateData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("8c039749-e2dc-12fc-0757-80e63a219052"),
                column: "Description",
                value: "Physics for Grade 11");

            migrationBuilder.UpdateData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("b862ea9a-d88a-6230-aeea-858a90d5a959"),
                column: "Description",
                value: "Biology for Grade 11");

            migrationBuilder.UpdateData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("caea75ab-707a-872b-c92f-5500afab7afb"),
                column: "Description",
                value: "Physics for Grade 10");

            migrationBuilder.UpdateData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("e37e03f0-c9ec-1714-3ff7-408956f8a8c0"),
                column: "Description",
                value: "Agriculture for Grade 10");

            migrationBuilder.UpdateData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("eb63371f-c483-4b34-cf09-1f1f6825f8ba"),
                column: "Description",
                value: "Mathematics for Grade 12");

            migrationBuilder.UpdateData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("f71b0725-2ab4-36b8-c08a-d709a40e16f4"),
                column: "Description",
                value: "Chemistry for Grade 11");

            migrationBuilder.UpdateData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("f8380400-ac39-9946-15e0-894ba7e520e6"),
                column: "Description",
                value: "Geography for Grade 10");

            migrationBuilder.UpdateData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("f9d10be1-9d0d-14ec-37e4-3ab4dfc611f8"),
                column: "Description",
                value: "Mathematics for Grade 10");

            migrationBuilder.InsertData(
                table: "Subjects",
                columns: new[] { "Id", "Color", "Description", "Grade", "Name" },
                values: new object[,]
                {
                    { new Guid("082d128a-6ee4-2c5d-0efc-f9b0b9af4c23"), "#22c55e", "Science for Grade 10", 10, "Science" },
                    { new Guid("532a8048-546e-3d68-aa5a-14ae71eb6f9e"), "#3b82f6", "English for Grade 12", 12, "English" },
                    { new Guid("82786b28-5a6d-b0b6-194d-ab131764064d"), "#22c55e", "Science for Grade 11", 11, "Science" },
                    { new Guid("89bdf99e-82a8-29b3-57d7-410b42d8dcf8"), "#22c55e", "Science for Grade 12", 12, "Science" },
                    { new Guid("99b68af6-34fc-713c-884f-c018c6b17d72"), "#3b82f6", "English for Grade 11", 11, "English" },
                    { new Guid("fdca9ad9-bc36-79d0-421e-38c118a6c3cf"), "#3b82f6", "English for Grade 10", 10, "English" }
                });

            migrationBuilder.UpdateData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("c86aaefa-61f2-1f82-6ecd-9243160261b4"),
                column: "Order",
                value: 6);

            migrationBuilder.InsertData(
                table: "Topics",
                columns: new[] { "Id", "Description", "Name", "Order", "SubjectId" },
                values: new object[,]
                {
                    { new Guid("050fd061-3d93-5f39-8a4a-c324dfe3ae55"), "Geomorphology (Surface Processes) — Geography Grade 10", "Geomorphology (Surface Processes)", 2, new Guid("f8380400-ac39-9946-15e0-894ba7e520e6") },
                    { new Guid("0618c3f0-291c-6ede-9cd8-2cb9f28e7d8c"), "Economic Geography (Secondary, Tertiary) — Geography Grade 11", "Economic Geography (Secondary, Tertiary)", 5, new Guid("629825b8-c2d2-7aee-d163-0be88495e272") },
                    { new Guid("0780c4e7-f729-4e93-dcd4-e97c01c6abe6"), "Animal Production (Breeding, Genetics) — Agriculture Grade 12", "Animal Production (Breeding, Genetics)", 3, new Guid("69e6f790-ae65-2e87-f351-982b99d92bfe") },
                    { new Guid("0b886e2a-37b2-806a-17ff-af7cbfd75b1f"), "Plant Studies (Structure, Growth) — Agriculture Grade 10", "Plant Studies (Structure, Growth)", 3, new Guid("e37e03f0-c9ec-1714-3ff7-408956f8a8c0") },
                    { new Guid("0b9021c9-d560-a851-92cc-d490c449b774"), "Resources and Sustainability (Energy) — Geography Grade 12", "Resources and Sustainability (Energy)", 6, new Guid("7a475323-7df5-f613-6449-fcfad4461d7c") },
                    { new Guid("0d051784-8b5c-c90f-7db0-eea0b82325e6"), "Biosphere to Ecosystems (Population Ecology) — Biology Grade 11", "Biosphere to Ecosystems (Population Ecology)", 6, new Guid("b862ea9a-d88a-6230-aeea-858a90d5a959") },
                    { new Guid("0f004646-1571-b5d5-f8fe-ae1d2a422952"), "Excretion in Humans — Biology Grade 10", "Excretion in Humans", 10, new Guid("6aaf47ef-e5a0-d4af-70db-f2011158ef8e") },
                    { new Guid("12c332eb-1189-88ff-d2cf-1491009c5e64"), "Electric Circuits (Advanced) — Physics Grade 12", "Electric Circuits (Advanced)", 6, new Guid("65ac38a6-5a9f-22e8-0de0-2181565e0d5e") },
                    { new Guid("197831c0-c31f-305f-5fca-6809f0d96f7f"), "Work, Energy and Power — Physics Grade 11", "Work, Energy and Power", 4, new Guid("8c039749-e2dc-12fc-0757-80e63a219052") },
                    { new Guid("214ed8ad-1898-327b-07df-f8ae143bbc41"), "Exam Preparation and Map Skills — Geography Grade 12", "Exam Preparation and Map Skills", 10, new Guid("7a475323-7df5-f613-6449-fcfad4461d7c") },
                    { new Guid("231ced44-3f9d-42fa-aa02-7ace491139be"), "Statistics — Mathematics Grade 10", "Statistics", 9, new Guid("f9d10be1-9d0d-14ec-37e4-3ab4dfc611f8") },
                    { new Guid("26436f95-e13e-e1db-1a17-740a78be7ce5"), "Finance and Growth — Mathematics Grade 10", "Finance and Growth", 5, new Guid("f9d10be1-9d0d-14ec-37e4-3ab4dfc611f8") },
                    { new Guid("2887d4df-b719-9850-3b71-d91448d76cd0"), "Electrodynamics (Motors, Generators) — Physics Grade 12", "Electrodynamics (Motors, Generators)", 7, new Guid("65ac38a6-5a9f-22e8-0de0-2181565e0d5e") },
                    { new Guid("288d8ea0-c1fb-b3e6-7972-dd554017287a"), "Plant Production (Advanced Techniques) — Agriculture Grade 12", "Plant Production (Advanced Techniques)", 2, new Guid("69e6f790-ae65-2e87-f351-982b99d92bfe") },
                    { new Guid("2a981624-ea72-0c90-5f56-cebe5168cb33"), "Tourism and the Environment — Geography Grade 11", "Tourism and the Environment", 10, new Guid("629825b8-c2d2-7aee-d163-0be88495e272") },
                    { new Guid("2d679eb7-979c-625c-f1d5-2a46d2eaba03"), "Magnetism and Electrostatics — Chemistry Grade 10", "Magnetism and Electrostatics", 6, new Guid("45c2e4c9-1dad-eb06-8108-5a7b253db61d") },
                    { new Guid("2d9f2e39-601b-8d59-3057-19d92171c6cf"), "Electrochemistry (Galvanic, Electrolytic Cells) — Chemistry Grade 12", "Electrochemistry (Galvanic, Electrolytic Cells)", 7, new Guid("26a00ceb-e8ed-7c0b-b245-a60b6107d495") },
                    { new Guid("2e1204bc-b7c0-831f-bf12-d8c8dbd271e5"), "Euclidean Geometry (Proportionality, Similarity) — Mathematics Grade 12", "Euclidean Geometry (Proportionality, Similarity)", 7, new Guid("eb63371f-c483-4b34-cf09-1f1f6825f8ba") },
                    { new Guid("2ec7ecd2-b943-bd77-8efc-b01725d6133a"), "Meiosis and Genetics — Biology Grade 12", "Meiosis and Genetics", 2, new Guid("06216eaa-a8e4-6710-e535-da8f3f9af209") },
                    { new Guid("2f03fddb-8947-45ea-224c-cd9e9cc8514e"), "Chemical Equilibrium (Introduction) — Chemistry Grade 11", "Chemical Equilibrium (Introduction)", 9, new Guid("f71b0725-2ab4-36b8-c08a-d709a40e16f4") },
                    { new Guid("32543d9f-dba6-d777-98b6-fb1248a26f82"), "Excretion in Humans (Urinary System) — Biology Grade 11", "Excretion in Humans (Urinary System)", 8, new Guid("b862ea9a-d88a-6230-aeea-858a90d5a959") },
                    { new Guid("333d6e57-1224-bdd4-f8ed-972af8e887dd"), "The Atmosphere and Weather — Geography Grade 10", "The Atmosphere and Weather", 1, new Guid("f8380400-ac39-9946-15e0-894ba7e520e6") },
                    { new Guid("34986eb4-6e8f-4ae3-c52b-8c4dd81799e3"), "Electrostatics (Electric Fields) — Physics Grade 12", "Electrostatics (Electric Fields)", 5, new Guid("65ac38a6-5a9f-22e8-0de0-2181565e0d5e") },
                    { new Guid("34ac8896-f538-98f6-045b-d6eac23845fc"), "Vertical Projectile Motion — Physics Grade 12", "Vertical Projectile Motion", 2, new Guid("65ac38a6-5a9f-22e8-0de0-2181565e0d5e") },
                    { new Guid("37cc5473-c3e9-fdb3-8066-e41147b900d3"), "Human Impact on the Environment — Biology Grade 12", "Human Impact on the Environment", 10, new Guid("06216eaa-a8e4-6710-e535-da8f3f9af209") },
                    { new Guid("394a31f0-6f35-1637-f69a-8cb7b9a306b5"), "Animal Studies (Classification, Nutrition) — Agriculture Grade 10", "Animal Studies (Classification, Nutrition)", 4, new Guid("e37e03f0-c9ec-1714-3ff7-408956f8a8c0") },
                    { new Guid("3b2fc17f-e7dc-f10e-6aef-d74f8f70428e"), "Water Resources — Geography Grade 10", "Water Resources", 5, new Guid("f8380400-ac39-9946-15e0-894ba7e520e6") },
                    { new Guid("3df6b272-c703-ea94-590d-39cf4c886223"), "Acids and Bases (pH, Titrations) — Chemistry Grade 12", "Acids and Bases (pH, Titrations)", 6, new Guid("26a00ceb-e8ed-7c0b-b245-a60b6107d495") },
                    { new Guid("418e8e0c-5c2c-7bb2-6827-b00bbf80bb39"), "Map Skills and Techniques — Geography Grade 10", "Map Skills and Techniques", 3, new Guid("f8380400-ac39-9946-15e0-894ba7e520e6") },
                    { new Guid("41c2f514-4908-cdb9-8cf8-ab12c6dd82bb"), "Atomic Combinations (Molecular Structure) — Chemistry Grade 11", "Atomic Combinations (Molecular Structure)", 1, new Guid("f71b0725-2ab4-36b8-c08a-d709a40e16f4") },
                    { new Guid("421ebeb7-f096-909d-bbed-88241ea8f5fd"), "Electric Circuits (Internal Resistance) — Physics Grade 11", "Electric Circuits (Internal Resistance)", 9, new Guid("8c039749-e2dc-12fc-0757-80e63a219052") },
                    { new Guid("45a27f2a-c0fe-2439-003e-bbb6b6a6fcf5"), "Environmental Impact of Agriculture — Agriculture Grade 11", "Environmental Impact of Agriculture", 9, new Guid("76324b9e-5fc2-7160-7522-932866c0a77a") },
                    { new Guid("46016da9-d7e5-79c3-42ed-7f9e781ad409"), "Gravity and Mechanical Energy — Physics Grade 10", "Gravity and Mechanical Energy", 4, new Guid("caea75ab-707a-872b-c92f-5500afab7afb") },
                    { new Guid("469a9107-71ed-3a47-6f04-4c66303ac5e3"), "Development Geography — Geography Grade 11", "Development Geography", 6, new Guid("629825b8-c2d2-7aee-d163-0be88495e272") },
                    { new Guid("48751395-d242-5f84-48c6-dd188eb243e3"), "Rate of Reactions (Factors, Mechanism) — Chemistry Grade 12", "Rate of Reactions (Factors, Mechanism)", 4, new Guid("26a00ceb-e8ed-7c0b-b245-a60b6107d495") },
                    { new Guid("4a7b80cb-3322-d226-5d04-7bef5cd1427d"), "Reproduction in Vertebrates — Biology Grade 11", "Reproduction in Vertebrates", 10, new Guid("b862ea9a-d88a-6230-aeea-858a90d5a959") },
                    { new Guid("504dfee0-22b2-3f84-66f8-0866506b2167"), "Physical and Chemical Change — Chemistry Grade 10", "Physical and Chemical Change", 4, new Guid("45c2e4c9-1dad-eb06-8108-5a7b253db61d") },
                    { new Guid("50de216c-0ddc-ae74-f8ad-5444e814b235"), "Biodiversity and Classification (Detailed) — Biology Grade 11", "Biodiversity and Classification (Detailed)", 1, new Guid("b862ea9a-d88a-6230-aeea-858a90d5a959") },
                    { new Guid("51359da3-e266-2b11-205d-1d3e634654c7"), "Population Studies — Geography Grade 10", "Population Studies", 4, new Guid("f8380400-ac39-9946-15e0-894ba7e520e6") },
                    { new Guid("51ef4d2e-6f88-4f2f-1fd2-185a6afe78b9"), "Equations and Inequalities — Mathematics Grade 11", "Equations and Inequalities", 2, new Guid("5295e6aa-8238-0794-a9ec-29deb0bef2aa") },
                    { new Guid("5262754b-d02f-be07-3cc5-81f92daf5e45"), "Animal Reproduction — Agriculture Grade 12", "Animal Reproduction", 4, new Guid("69e6f790-ae65-2e87-f351-982b99d92bfe") },
                    { new Guid("52a5fcb0-3134-3b41-f1ae-d181ba532f09"), "Analytical Geometry — Mathematics Grade 10", "Analytical Geometry", 7, new Guid("f9d10be1-9d0d-14ec-37e4-3ab4dfc611f8") },
                    { new Guid("532239c5-336a-48f8-cf92-a9651fc66796"), "Light (Refraction, Snell's Law) — Physics Grade 11", "Light (Refraction, Snell's Law)", 7, new Guid("8c039749-e2dc-12fc-0757-80e63a219052") },
                    { new Guid("53da0239-03de-447c-e7d5-0ac404ee6ce8"), "Gaseous Exchange — Biology Grade 10", "Gaseous Exchange", 9, new Guid("6aaf47ef-e5a0-d4af-70db-f2011158ef8e") },
                    { new Guid("54110ec1-7c50-6e4c-b811-c4dd1df5485a"), "Electric Circuits — Physics Grade 10", "Electric Circuits", 9, new Guid("caea75ab-707a-872b-c92f-5500afab7afb") },
                    { new Guid("58648486-b170-4c7d-f262-5ae496ea71de"), "Endocrine System — Biology Grade 12", "Endocrine System", 6, new Guid("06216eaa-a8e4-6710-e535-da8f3f9af209") },
                    { new Guid("58ecad01-ddfb-38eb-e236-388b371ea195"), "Statistics (Counting Principles) — Mathematics Grade 12", "Statistics (Counting Principles)", 8, new Guid("eb63371f-c483-4b34-cf09-1f1f6825f8ba") },
                    { new Guid("59d26f10-1011-730a-bb2a-da01ad019205"), "Rate of Reactions — Chemistry Grade 11", "Rate of Reactions", 8, new Guid("f71b0725-2ab4-36b8-c08a-d709a40e16f4") },
                    { new Guid("5a6e7f96-d196-5905-45a2-408144c11403"), "Algebraic Expressions — Mathematics Grade 10", "Algebraic Expressions", 1, new Guid("f9d10be1-9d0d-14ec-37e4-3ab4dfc611f8") },
                    { new Guid("5a75752a-5a4c-ab5e-cc8f-b5ec4d3cea1b"), "Optical Phenomena (Photoelectric Effect) — Physics Grade 12", "Optical Phenomena (Photoelectric Effect)", 8, new Guid("65ac38a6-5a9f-22e8-0de0-2181565e0d5e") },
                    { new Guid("5a765634-6417-4f5e-1a33-b45063aa04f3"), "Organic Chemistry (Polymers, Plastics) — Chemistry Grade 12", "Organic Chemistry (Polymers, Plastics)", 3, new Guid("26a00ceb-e8ed-7c0b-b245-a60b6107d495") },
                    { new Guid("5ea08431-8e62-9a42-e6d8-2be18c18db18"), "Sound (Doppler Effect intro) — Physics Grade 11", "Sound (Doppler Effect intro)", 6, new Guid("8c039749-e2dc-12fc-0757-80e63a219052") },
                    { new Guid("5fbc9b9f-3366-4659-dd89-9cf00a28b73a"), "Quantitative Aspects (Mole Concept) — Chemistry Grade 10", "Quantitative Aspects (Mole Concept)", 9, new Guid("45c2e4c9-1dad-eb06-8108-5a7b253db61d") },
                    { new Guid("60f06aab-ede4-cd69-1197-dfe54ecd27ec"), "Nervous System and Senses — Biology Grade 12", "Nervous System and Senses", 5, new Guid("06216eaa-a8e4-6710-e535-da8f3f9af209") },
                    { new Guid("61542b76-fc35-064b-7dfb-ed661ab26961"), "Homeostasis — Biology Grade 12", "Homeostasis", 7, new Guid("06216eaa-a8e4-6710-e535-da8f3f9af209") },
                    { new Guid("615c5357-e20c-8d6a-db74-dc5e43b353ad"), "Soil Science (Formation, Types) — Agriculture Grade 10", "Soil Science (Formation, Types)", 2, new Guid("e37e03f0-c9ec-1714-3ff7-408956f8a8c0") },
                    { new Guid("6288f6fc-0672-c122-b2b9-df5d967376bc"), "Plant Production (Crop Management) — Agriculture Grade 11", "Plant Production (Crop Management)", 2, new Guid("76324b9e-5fc2-7160-7522-932866c0a77a") },
                    { new Guid("63ad600b-8bca-1b26-eade-64d0a219a279"), "Functions and Graphs — Mathematics Grade 10", "Functions and Graphs", 4, new Guid("f9d10be1-9d0d-14ec-37e4-3ab4dfc611f8") },
                    { new Guid("6d43e975-bfc9-fca1-c243-de13a1063642"), "Units and Measurements — Physics Grade 10", "Units and Measurements", 1, new Guid("caea75ab-707a-872b-c92f-5500afab7afb") },
                    { new Guid("6dfcc605-241e-9b04-b6bb-35aed8cc13cc"), "Magnetism — Physics Grade 10", "Magnetism", 10, new Guid("caea75ab-707a-872b-c92f-5500afab7afb") },
                    { new Guid("6ef9764f-16b5-a8de-e373-6f9f7bbf35e4"), "Cell Division (Meiosis) — Biology Grade 11", "Cell Division (Meiosis)", 9, new Guid("b862ea9a-d88a-6230-aeea-858a90d5a959") },
                    { new Guid("6f52470d-b0fa-604c-ad84-15bae8e1a8b2"), "Quantitative Aspects (Stoichiometry) — Chemistry Grade 11", "Quantitative Aspects (Stoichiometry)", 4, new Guid("f71b0725-2ab4-36b8-c08a-d709a40e16f4") },
                    { new Guid("70d9d2a8-636e-3488-4edb-cc64cf300f77"), "Finance, Growth and Decay (Annuities) — Mathematics Grade 12", "Finance, Growth and Decay (Annuities)", 3, new Guid("eb63371f-c483-4b34-cf09-1f1f6825f8ba") },
                    { new Guid("70f888ef-b3d2-f258-7cbe-3feab9adba0d"), "Solutions and Solubility — Chemistry Grade 10", "Solutions and Solubility", 8, new Guid("45c2e4c9-1dad-eb06-8108-5a7b253db61d") },
                    { new Guid("716f6983-9029-6713-64bd-776749d4500c"), "Momentum — Physics Grade 11", "Momentum", 3, new Guid("8c039749-e2dc-12fc-0757-80e63a219052") },
                    { new Guid("718dcda4-4d62-267f-8ac8-f05a1c9d2726"), "Human Evolution — Biology Grade 12", "Human Evolution", 4, new Guid("06216eaa-a8e4-6710-e535-da8f3f9af209") },
                    { new Guid("72272db0-2456-e9d7-4e47-7eab7c5a961a"), "Equations and Inequalities — Mathematics Grade 10", "Equations and Inequalities", 2, new Guid("f9d10be1-9d0d-14ec-37e4-3ab4dfc611f8") },
                    { new Guid("72499899-2012-50f0-baca-b8267f238437"), "Statistics (Regression, Correlation) — Mathematics Grade 11", "Statistics (Regression, Correlation)", 9, new Guid("5295e6aa-8238-0794-a9ec-29deb0bef2aa") },
                    { new Guid("7431c6fe-376b-4b07-b480-d36203e1e9df"), "Energy and Chemical Change — Chemistry Grade 10", "Energy and Chemical Change", 10, new Guid("45c2e4c9-1dad-eb06-8108-5a7b253db61d") },
                    { new Guid("7482d60f-e6a9-4de5-e0c0-9a664877752f"), "Settlement Geography — Geography Grade 10", "Settlement Geography", 7, new Guid("f8380400-ac39-9946-15e0-894ba7e520e6") },
                    { new Guid("74ebf86b-b31b-5327-dc77-d43bc9f9ee18"), "Chemical Equilibrium (Le Chatelier's) — Chemistry Grade 12", "Chemical Equilibrium (Le Chatelier's)", 5, new Guid("26a00ceb-e8ed-7c0b-b245-a60b6107d495") },
                    { new Guid("76040032-7e9e-5cc1-a3a8-41225f37ed3b"), "Trigonometry (Compound and Double Angles) — Mathematics Grade 12", "Trigonometry (Compound and Double Angles)", 5, new Guid("eb63371f-c483-4b34-cf09-1f1f6825f8ba") },
                    { new Guid("76f4b310-7843-c068-1452-a3c0c8352ade"), "Agricultural Economics (Business Plans) — Agriculture Grade 12", "Agricultural Economics (Business Plans)", 5, new Guid("69e6f790-ae65-2e87-f351-982b99d92bfe") },
                    { new Guid("771514d7-f986-ab4f-9253-413fb16300f1"), "Energy and Chemical Change (Enthalpy) — Chemistry Grade 11", "Energy and Chemical Change (Enthalpy)", 5, new Guid("f71b0725-2ab4-36b8-c08a-d709a40e16f4") },
                    { new Guid("781aede2-451d-e8a1-0583-bdb2657a62b4"), "Organic Chemistry (Nomenclature) — Chemistry Grade 12", "Organic Chemistry (Nomenclature)", 1, new Guid("26a00ceb-e8ed-7c0b-b245-a60b6107d495") },
                    { new Guid("792e1ddb-b494-4edd-dbfe-0e51a35071d5"), "Biosphere to Ecosystems — Biology Grade 10", "Biosphere to Ecosystems", 7, new Guid("6aaf47ef-e5a0-d4af-70db-f2011158ef8e") },
                    { new Guid("7a6038e9-d2e5-cbc0-fe36-4d39c79939a5"), "Forces and Newton's Laws — Physics Grade 10", "Forces and Newton's Laws", 3, new Guid("caea75ab-707a-872b-c92f-5500afab7afb") },
                    { new Guid("7d8d7a19-7d81-d3c9-47c7-ac7cebadf41a"), "Euclidean Geometry (Circle Theorems) — Mathematics Grade 11", "Euclidean Geometry (Circle Theorems)", 8, new Guid("5295e6aa-8238-0794-a9ec-29deb0bef2aa") },
                    { new Guid("7dfd8bfa-4134-5dc1-7041-04661e427d6b"), "Chemical Bonding — Chemistry Grade 10", "Chemical Bonding", 3, new Guid("45c2e4c9-1dad-eb06-8108-5a7b253db61d") },
                    { new Guid("7f275686-7a4e-77cf-fb60-f5e7020f1a06"), "Climate and Weather (South Africa) — Geography Grade 10", "Climate and Weather (South Africa)", 6, new Guid("f8380400-ac39-9946-15e0-894ba7e520e6") },
                    { new Guid("81004de3-ba78-1bbd-8090-be0cda89866c"), "Representing Chemical Change — Chemistry Grade 10", "Representing Chemical Change", 5, new Guid("45c2e4c9-1dad-eb06-8108-5a7b253db61d") },
                    { new Guid("81788eec-1a1c-14e9-548c-44f7e31e74c4"), "Water Management (Irrigation) — Agriculture Grade 10", "Water Management (Irrigation)", 7, new Guid("e37e03f0-c9ec-1714-3ff7-408956f8a8c0") },
                    { new Guid("82b597dd-e1dc-3398-27ac-c4a9bbe80b44"), "Hazards and Disasters (Risk Management) — Geography Grade 12", "Hazards and Disasters (Risk Management)", 7, new Guid("7a475323-7df5-f613-6449-fcfad4461d7c") },
                    { new Guid("83321fec-3af1-8082-dbbf-9aa7cf4acebb"), "Resources and Sustainability — Geography Grade 11", "Resources and Sustainability", 7, new Guid("629825b8-c2d2-7aee-d163-0be88495e272") },
                    { new Guid("839e9bb7-cb14-f554-d1c5-cd0dde7f5784"), "Population (Migration, Urbanization) — Geography Grade 11", "Population (Migration, Urbanization)", 4, new Guid("629825b8-c2d2-7aee-d163-0be88495e272") },
                    { new Guid("854a8a9e-7b89-c9fb-4811-4c5d9d19023b"), "Gaseous Exchange (Detailed) — Biology Grade 11", "Gaseous Exchange (Detailed)", 7, new Guid("b862ea9a-d88a-6230-aeea-858a90d5a959") },
                    { new Guid("867d475d-0238-17ca-510c-320f7f616248"), "Soil Science (Advanced, Land Use) — Agriculture Grade 12", "Soil Science (Advanced, Land Use)", 1, new Guid("69e6f790-ae65-2e87-f351-982b99d92bfe") },
                    { new Guid("86ce8d84-408d-9d4c-adf2-8d16d69423e4"), "Types of Reactions (Acid-Base, Redox) — Chemistry Grade 11", "Types of Reactions (Acid-Base, Redox)", 6, new Guid("f71b0725-2ab4-36b8-c08a-d709a40e16f4") },
                    { new Guid("88d3aed4-7aa6-aeaf-be74-3f3c44f8fe91"), "Response of the Immune System — Biology Grade 12", "Response of the Immune System", 9, new Guid("06216eaa-a8e4-6710-e535-da8f3f9af209") },
                    { new Guid("8a4cb3cf-d79d-8b4e-e9cc-bd45a903fb90"), "Cell Structure and Function — Biology Grade 10", "Cell Structure and Function", 2, new Guid("6aaf47ef-e5a0-d4af-70db-f2011158ef8e") },
                    { new Guid("8b2a8163-39ae-a6f0-d25d-5c05a22bd365"), "Exam Revision and Problem Solving — Chemistry Grade 12", "Exam Revision and Problem Solving", 10, new Guid("26a00ceb-e8ed-7c0b-b245-a60b6107d495") },
                    { new Guid("8b929c5a-3418-4a3f-982c-47939f2de55b"), "Periodic Table — Chemistry Grade 10", "Periodic Table", 2, new Guid("45c2e4c9-1dad-eb06-8108-5a7b253db61d") },
                    { new Guid("8d715e79-be6e-5e13-10c9-2185d6fa5076"), "Plant and Animal Tissues (Advanced) — Biology Grade 11", "Plant and Animal Tissues (Advanced)", 3, new Guid("b862ea9a-d88a-6230-aeea-858a90d5a959") },
                    { new Guid("8d739583-8bb6-1144-d638-3130c8670a85"), "Doppler Effect — Physics Grade 12", "Doppler Effect", 4, new Guid("65ac38a6-5a9f-22e8-0de0-2181565e0d5e") },
                    { new Guid("8df031a3-dc4a-74cb-8cea-84482e97e0cc"), "Farm Planning and Layout — Agriculture Grade 10", "Farm Planning and Layout", 6, new Guid("e37e03f0-c9ec-1714-3ff7-408956f8a8c0") },
                    { new Guid("8fe6402d-ab28-170b-b190-59519649e6a0"), "Measurement — Mathematics Grade 10", "Measurement", 11, new Guid("f9d10be1-9d0d-14ec-37e4-3ab4dfc611f8") },
                    { new Guid("900b0581-a884-0700-5c2f-1ee846b2d9a6"), "Environmental Geography — Geography Grade 10", "Environmental Geography", 9, new Guid("f8380400-ac39-9946-15e0-894ba7e520e6") },
                    { new Guid("91d90182-e3de-3d69-34d2-fd07abb6710a"), "Farm Management (Labour, Finance) — Agriculture Grade 12", "Farm Management (Labour, Finance)", 6, new Guid("69e6f790-ae65-2e87-f351-982b99d92bfe") },
                    { new Guid("92141092-e9bc-e505-2aca-35d46b603441"), "Sequences and Series — Mathematics Grade 12", "Sequences and Series", 1, new Guid("eb63371f-c483-4b34-cf09-1f1f6825f8ba") },
                    { new Guid("921d990b-d5de-edf1-c136-335b1b60a23d"), "Geomorphology (Fluvial, Coastal Landscapes) — Geography Grade 12", "Geomorphology (Fluvial, Coastal Landscapes)", 2, new Guid("7a475323-7df5-f613-6449-fcfad4461d7c") },
                    { new Guid("92579598-4144-1b60-efe1-d108270bee32"), "Spatial Planning — Geography Grade 12", "Spatial Planning", 9, new Guid("7a475323-7df5-f613-6449-fcfad4461d7c") },
                    { new Guid("93891937-80ee-7db9-5ded-c76f297baf24"), "Introduction to Agriculture — Agriculture Grade 10", "Introduction to Agriculture", 1, new Guid("e37e03f0-c9ec-1714-3ff7-408956f8a8c0") },
                    { new Guid("93dd70db-f471-0287-6b4a-088b0afdc4e9"), "Work, Energy and Power (Advanced) — Physics Grade 12", "Work, Energy and Power (Advanced)", 3, new Guid("65ac38a6-5a9f-22e8-0de0-2181565e0d5e") },
                    { new Guid("94a0ed13-a568-1e7f-94d6-a362cbb000a2"), "Finance, Growth and Decay — Mathematics Grade 11", "Finance, Growth and Decay", 5, new Guid("5295e6aa-8238-0794-a9ec-29deb0bef2aa") },
                    { new Guid("958ef24a-1848-11fc-7260-6ca5fb0e08b5"), "Map Work and Calculations — Geography Grade 11", "Map Work and Calculations", 3, new Guid("629825b8-c2d2-7aee-d163-0be88495e272") },
                    { new Guid("972dc873-9109-24de-9ff9-fd076b1783d4"), "Momentum and Impulse (Advanced) — Physics Grade 12", "Momentum and Impulse (Advanced)", 1, new Guid("65ac38a6-5a9f-22e8-0de0-2181565e0d5e") },
                    { new Guid("982a1d25-91f5-061f-eec2-903b4fd07a5b"), "Nuclear Physics — Physics Grade 12", "Nuclear Physics", 10, new Guid("65ac38a6-5a9f-22e8-0de0-2181565e0d5e") },
                    { new Guid("98dd9392-0a4f-01ca-5bf8-3070c618379b"), "Quantitative Analysis — Chemistry Grade 12", "Quantitative Analysis", 9, new Guid("26a00ceb-e8ed-7c0b-b245-a60b6107d495") },
                    { new Guid("9a7e23fa-7cd8-7df9-3ebf-04d780ad7bb8"), "GIS and Remote Sensing (Introduction) — Geography Grade 10", "GIS and Remote Sensing (Introduction)", 10, new Guid("f8380400-ac39-9946-15e0-894ba7e520e6") },
                    { new Guid("9ac07bb9-773c-3e1a-3dbf-fd306f613c94"), "Climate and Weather (Global Patterns) — Geography Grade 11", "Climate and Weather (Global Patterns)", 1, new Guid("629825b8-c2d2-7aee-d163-0be88495e272") },
                    { new Guid("9b431cb9-474e-8611-1199-e207dd49740a"), "Probability (Tree Diagrams, Contingency Tables) — Mathematics Grade 11", "Probability (Tree Diagrams, Contingency Tables)", 10, new Guid("5295e6aa-8238-0794-a9ec-29deb0bef2aa") },
                    { new Guid("9c623dd8-3a7c-8aad-0601-f0a84074c3e9"), "Electromagnetic Radiation (Atomic Spectra) — Physics Grade 12", "Electromagnetic Radiation (Atomic Spectra)", 9, new Guid("65ac38a6-5a9f-22e8-0de0-2181565e0d5e") },
                    { new Guid("9d5c3fe7-8b62-b3c7-dde8-82fb9da0350b"), "Analytical Geometry (Circles, Tangents) — Mathematics Grade 12", "Analytical Geometry (Circles, Tangents)", 6, new Guid("eb63371f-c483-4b34-cf09-1f1f6825f8ba") },
                    { new Guid("9f3ff1df-ad72-2908-3f6b-2bc02348a496"), "Geomorphology (Drainage Systems) — Geography Grade 11", "Geomorphology (Drainage Systems)", 2, new Guid("629825b8-c2d2-7aee-d163-0be88495e272") },
                    { new Guid("9fa00bb7-65bf-0e73-7a34-d9f776af9cdb"), "Animal Production (Livestock Management) — Agriculture Grade 11", "Animal Production (Livestock Management)", 3, new Guid("76324b9e-5fc2-7160-7522-932866c0a77a") },
                    { new Guid("a229c887-61eb-61c3-82a1-8fc37af2a58e"), "Economic Geography of South Africa — Geography Grade 12", "Economic Geography of South Africa", 4, new Guid("7a475323-7df5-f613-6449-fcfad4461d7c") },
                    { new Guid("a27e3586-6d0e-ba1e-0584-df1d3557d392"), "Number Patterns and Sequences — Mathematics Grade 11", "Number Patterns and Sequences", 3, new Guid("5295e6aa-8238-0794-a9ec-29deb0bef2aa") },
                    { new Guid("a3de0b84-1ab3-2f97-5ddb-2a72dffd9fb2"), "Evolution (Natural Selection, Speciation) — Biology Grade 12", "Evolution (Natural Selection, Speciation)", 3, new Guid("06216eaa-a8e4-6710-e535-da8f3f9af209") },
                    { new Guid("a467b2c6-0c89-9bf5-2d65-016d07ea87e3"), "Hazards and Disasters — Geography Grade 11", "Hazards and Disasters", 8, new Guid("629825b8-c2d2-7aee-d163-0be88495e272") },
                    { new Guid("a4c9b011-4ccd-76c9-ddbf-7290777f735f"), "Indigenous Knowledge Systems in Agriculture — Agriculture Grade 11", "Indigenous Knowledge Systems in Agriculture", 10, new Guid("76324b9e-5fc2-7160-7522-932866c0a77a") },
                    { new Guid("a82eb8a4-4575-69b4-27ba-42261f0dd677"), "Energy Flow and Nutrient Cycling — Biology Grade 10", "Energy Flow and Nutrient Cycling", 8, new Guid("6aaf47ef-e5a0-d4af-70db-f2011158ef8e") },
                    { new Guid("a96f56d1-fcda-bdb6-e26a-add35e069baf"), "Support and Transport Systems in Plants — Biology Grade 11", "Support and Transport Systems in Plants", 4, new Guid("b862ea9a-d88a-6230-aeea-858a90d5a959") },
                    { new Guid("aad284ca-133a-8645-69d8-b8b3d90e1cb9"), "Intermolecular Forces (Advanced) — Chemistry Grade 11", "Intermolecular Forces (Advanced)", 2, new Guid("f71b0725-2ab4-36b8-c08a-d709a40e16f4") },
                    { new Guid("ab210d31-92e1-b37c-ef4a-aaa6e59542d8"), "Economic Geography (Primary Activities) — Geography Grade 10", "Economic Geography (Primary Activities)", 8, new Guid("f8380400-ac39-9946-15e0-894ba7e520e6") },
                    { new Guid("adaef9b7-1a85-d8bc-c690-83288f70c2d6"), "Functions (Parabola, Hyperbola, Exponential) — Mathematics Grade 11", "Functions (Parabola, Hyperbola, Exponential)", 4, new Guid("5295e6aa-8238-0794-a9ec-29deb0bef2aa") },
                    { new Guid("ae58b202-01db-0196-287e-57f047e49539"), "Differential Calculus — Mathematics Grade 12", "Differential Calculus", 4, new Guid("eb63371f-c483-4b34-cf09-1f1f6825f8ba") },
                    { new Guid("af4be5ec-c9c7-cb98-28e8-555888526a6a"), "Mechanization and Technology — Agriculture Grade 11", "Mechanization and Technology", 8, new Guid("76324b9e-5fc2-7160-7522-932866c0a77a") },
                    { new Guid("af6df267-d7f7-40aa-bac4-97a62cdb4812"), "Functions and Inverse Functions — Mathematics Grade 12", "Functions and Inverse Functions", 2, new Guid("eb63371f-c483-4b34-cf09-1f1f6825f8ba") },
                    { new Guid("b44f7958-dee5-6736-648c-6e871f906d3f"), "Vectors in Two Dimensions — Physics Grade 11", "Vectors in Two Dimensions", 1, new Guid("8c039749-e2dc-12fc-0757-80e63a219052") },
                    { new Guid("b4a4b05f-7229-0b1e-5112-5fbe330f87c5"), "Euclidean Geometry — Mathematics Grade 10", "Euclidean Geometry", 8, new Guid("f9d10be1-9d0d-14ec-37e4-3ab4dfc611f8") },
                    { new Guid("bb7b2c32-22c4-7ee8-84bb-1d80f3312ea3"), "Trigonometry — Mathematics Grade 10", "Trigonometry", 6, new Guid("f9d10be1-9d0d-14ec-37e4-3ab4dfc611f8") },
                    { new Guid("bbf12c25-de6f-1e64-1c1a-4cd09ccdc5c7"), "Biodiversity and Classification — Biology Grade 10", "Biodiversity and Classification", 5, new Guid("6aaf47ef-e5a0-d4af-70db-f2011158ef8e") },
                    { new Guid("bf10904f-67bc-7ab4-8118-404a265badc2"), "Electromagnetism — Physics Grade 11", "Electromagnetism", 10, new Guid("8c039749-e2dc-12fc-0757-80e63a219052") },
                    { new Guid("bff01c9b-9947-3576-9732-8ebe11eabebd"), "DNA, RNA and Protein Synthesis — Biology Grade 12", "DNA, RNA and Protein Synthesis", 1, new Guid("06216eaa-a8e4-6710-e535-da8f3f9af209") },
                    { new Guid("c094121f-5733-f5bf-3495-25546af2bc95"), "Atomic Structure — Chemistry Grade 10", "Atomic Structure", 1, new Guid("45c2e4c9-1dad-eb06-8108-5a7b253db61d") },
                    { new Guid("c0a479ce-2f71-6717-c032-3daaf8641b3a"), "Intermolecular Forces (Intro) — Chemistry Grade 10", "Intermolecular Forces (Intro)", 7, new Guid("45c2e4c9-1dad-eb06-8108-5a7b253db61d") },
                    { new Guid("c0e20d79-e15b-7eac-ce2c-2c225464fbc7"), "Transverse Pulses and Waves — Physics Grade 10", "Transverse Pulses and Waves", 5, new Guid("caea75ab-707a-872b-c92f-5500afab7afb") },
                    { new Guid("c15d5a7b-0450-1976-7333-fb5cb0d0a05c"), "Climate Change and Agriculture — Agriculture Grade 12", "Climate Change and Agriculture", 8, new Guid("69e6f790-ae65-2e87-f351-982b99d92bfe") },
                    { new Guid("c2c2dc37-85b5-b179-a185-3a817633d54a"), "Soil Science (Fertility, Conservation) — Agriculture Grade 11", "Soil Science (Fertility, Conservation)", 1, new Guid("76324b9e-5fc2-7160-7522-932866c0a77a") },
                    { new Guid("c306c6c6-0139-cef5-3df7-8ccaaf61903c"), "Electrochemistry (Introduction) — Chemistry Grade 11", "Electrochemistry (Introduction)", 10, new Guid("f71b0725-2ab4-36b8-c08a-d709a40e16f4") },
                    { new Guid("c593755e-43a2-bee1-efd8-ab112688b7d3"), "Support and Transport Systems in Animals — Biology Grade 11", "Support and Transport Systems in Animals", 5, new Guid("b862ea9a-d88a-6230-aeea-858a90d5a959") },
                    { new Guid("c5dfcf02-6cc0-0435-efd5-4a5936dbaa16"), "Entrepreneurship in Agriculture — Agriculture Grade 12", "Entrepreneurship in Agriculture", 10, new Guid("69e6f790-ae65-2e87-f351-982b99d92bfe") },
                    { new Guid("c5e93b39-4278-eae7-3ac5-22b60c087068"), "Probability — Mathematics Grade 10", "Probability", 10, new Guid("f9d10be1-9d0d-14ec-37e4-3ab4dfc611f8") },
                    { new Guid("c677ac5a-d29a-8fb7-a71d-3986e156e8ab"), "GIS (Advanced Applications) — Geography Grade 12", "GIS (Advanced Applications)", 8, new Guid("7a475323-7df5-f613-6449-fcfad4461d7c") },
                    { new Guid("c6cf9add-0ec4-5b53-e4f8-65a523a9749b"), "Farm Management and Planning — Agriculture Grade 11", "Farm Management and Planning", 7, new Guid("76324b9e-5fc2-7160-7522-932866c0a77a") },
                    { new Guid("c8259e20-c95c-2239-fd1f-c9d15bfababc"), "The Chemical Industry (Fertilizers) — Chemistry Grade 12", "The Chemical Industry (Fertilizers)", 8, new Guid("26a00ceb-e8ed-7c0b-b245-a60b6107d495") },
                    { new Guid("c8ad3461-3649-93da-ed28-595297e891c3"), "Agricultural Economics (Basic) — Agriculture Grade 10", "Agricultural Economics (Basic)", 5, new Guid("e37e03f0-c9ec-1714-3ff7-408956f8a8c0") },
                    { new Guid("c8e3da43-f33b-c2f1-d9db-f6553cf6ce64"), "Motion in One Dimension — Physics Grade 10", "Motion in One Dimension", 2, new Guid("caea75ab-707a-872b-c92f-5500afab7afb") },
                    { new Guid("c908eff7-1df5-09a9-f5ae-3617899b6f31"), "Probability (Fundamental Counting Principle) — Mathematics Grade 12", "Probability (Fundamental Counting Principle)", 9, new Guid("eb63371f-c483-4b34-cf09-1f1f6825f8ba") },
                    { new Guid("c99c9b78-3b6b-5e2f-d6b3-57cb0918ec66"), "Pest and Disease Control — Agriculture Grade 10", "Pest and Disease Control", 8, new Guid("e37e03f0-c9ec-1714-3ff7-408956f8a8c0") },
                    { new Guid("cabfdf95-7580-c4f0-9d96-e354a9d1d823"), "Waves (Interference, Diffraction) — Physics Grade 11", "Waves (Interference, Diffraction)", 5, new Guid("8c039749-e2dc-12fc-0757-80e63a219052") },
                    { new Guid("cc0faa1b-b9b5-49b5-1cc2-9514bdab0a64"), "Chemistry of Life — Biology Grade 10", "Chemistry of Life", 1, new Guid("6aaf47ef-e5a0-d4af-70db-f2011158ef8e") },
                    { new Guid("cd33959b-8ba9-cb96-3898-34c93816e98f"), "Newton's Laws (Applications) — Physics Grade 11", "Newton's Laws (Applications)", 2, new Guid("8c039749-e2dc-12fc-0757-80e63a219052") },
                    { new Guid("d03460f3-f948-4426-bdbf-8c091519d492"), "Number Patterns — Mathematics Grade 10", "Number Patterns", 3, new Guid("f9d10be1-9d0d-14ec-37e4-3ab4dfc611f8") },
                    { new Guid("d4770125-312b-dd21-7945-d24510e977f5"), "Ideal Gases (Gas Laws) — Chemistry Grade 11", "Ideal Gases (Gas Laws)", 3, new Guid("f71b0725-2ab4-36b8-c08a-d709a40e16f4") },
                    { new Guid("d57cb817-d7f3-efd6-aae8-ea320ccba361"), "GIS Applications — Geography Grade 11", "GIS Applications", 9, new Guid("629825b8-c2d2-7aee-d163-0be88495e272") },
                    { new Guid("d8eb0352-ad2a-b199-aa10-a4db9eb9e308"), "Animal Health and Diseases — Agriculture Grade 11", "Animal Health and Diseases", 5, new Guid("76324b9e-5fc2-7160-7522-932866c0a77a") },
                    { new Guid("db34d196-bee5-f55a-ac42-12b3644bca05"), "Agricultural Technology — Agriculture Grade 10", "Agricultural Technology", 10, new Guid("e37e03f0-c9ec-1714-3ff7-408956f8a8c0") },
                    { new Guid("ddeb7bb0-2796-b4f5-151b-55ad39bd4715"), "Electromagnetic Radiation — Physics Grade 10", "Electromagnetic Radiation", 7, new Guid("caea75ab-707a-872b-c92f-5500afab7afb") },
                    { new Guid("df30a84d-9cb6-128b-745f-2a71a232f4ec"), "The Lithosphere (Mining, Energy) — Chemistry Grade 11", "The Lithosphere (Mining, Energy)", 7, new Guid("f71b0725-2ab4-36b8-c08a-d709a40e16f4") },
                    { new Guid("df9b8207-2991-efd2-9864-5cd2fe8d486f"), "Development Geography (Global Issues) — Geography Grade 12", "Development Geography (Global Issues)", 5, new Guid("7a475323-7df5-f613-6449-fcfad4461d7c") },
                    { new Guid("e035f078-80d9-779b-beca-f25499bf63c4"), "Climate and Weather (Synoptic Charts) — Geography Grade 12", "Climate and Weather (Synoptic Charts)", 1, new Guid("7a475323-7df5-f613-6449-fcfad4461d7c") },
                    { new Guid("e4e71981-b951-05a5-37af-a4c1ab890849"), "History of Life on Earth — Biology Grade 10", "History of Life on Earth", 6, new Guid("6aaf47ef-e5a0-d4af-70db-f2011158ef8e") },
                    { new Guid("e5ab0208-183d-a059-c5ff-3c862c31674a"), "Electrostatics (Coulomb's Law) — Physics Grade 11", "Electrostatics (Coulomb's Law)", 8, new Guid("8c039749-e2dc-12fc-0757-80e63a219052") },
                    { new Guid("e5ea993b-9f92-26fd-9449-25fb109b2340"), "Electrostatics — Physics Grade 10", "Electrostatics", 8, new Guid("caea75ab-707a-872b-c92f-5500afab7afb") },
                    { new Guid("e624014c-6042-8ce8-ebc8-21db8dffceee"), "Agricultural Economics (Markets, Pricing) — Agriculture Grade 11", "Agricultural Economics (Markets, Pricing)", 6, new Guid("76324b9e-5fc2-7160-7522-932866c0a77a") },
                    { new Guid("ea9ecd93-dd47-0dfc-805c-d22f1422484f"), "Exponents and Surds — Mathematics Grade 11", "Exponents and Surds", 1, new Guid("5295e6aa-8238-0794-a9ec-29deb0bef2aa") },
                    { new Guid("eab93840-1960-cecc-c7c8-928cd373fc22"), "Trigonometry (Identities, Equations, Graphs) — Mathematics Grade 11", "Trigonometry (Identities, Equations, Graphs)", 6, new Guid("5295e6aa-8238-0794-a9ec-29deb0bef2aa") },
                    { new Guid("ecb99fdd-8c39-dba9-8237-2e95125945f8"), "History of Life on Earth (Evolution) — Biology Grade 11", "History of Life on Earth (Evolution)", 2, new Guid("b862ea9a-d88a-6230-aeea-858a90d5a959") },
                    { new Guid("ed750b69-66a5-81ae-b3ad-912c2e5e7ba3"), "Sustainable Farming Practices — Agriculture Grade 10", "Sustainable Farming Practices", 9, new Guid("e37e03f0-c9ec-1714-3ff7-408956f8a8c0") },
                    { new Guid("f36dfb07-4b90-fba9-f074-15dc22a07029"), "Plant and Animal Tissues — Biology Grade 10", "Plant and Animal Tissues", 4, new Guid("6aaf47ef-e5a0-d4af-70db-f2011158ef8e") },
                    { new Guid("f38c2c9d-e19b-8359-480c-67c3669b662f"), "Organic Chemistry (Reactions) — Chemistry Grade 12", "Organic Chemistry (Reactions)", 2, new Guid("26a00ceb-e8ed-7c0b-b245-a60b6107d495") },
                    { new Guid("f39fa9a8-b9fe-a19d-c71a-b9350eecf920"), "Food Security and Safety — Agriculture Grade 12", "Food Security and Safety", 9, new Guid("69e6f790-ae65-2e87-f351-982b99d92bfe") },
                    { new Guid("f57ba5bd-1825-3bfd-616a-861b4eeae95d"), "Agro-Processing and Beneficiation — Agriculture Grade 12", "Agro-Processing and Beneficiation", 7, new Guid("69e6f790-ae65-2e87-f351-982b99d92bfe") },
                    { new Guid("f5ab6ef6-952a-47fa-81a2-1aa7aeb10694"), "Human Reproduction — Biology Grade 12", "Human Reproduction", 8, new Guid("06216eaa-a8e4-6710-e535-da8f3f9af209") },
                    { new Guid("f715ea05-b1ce-5c66-95f0-5a7ad3e6fed6"), "Map Work (Advanced Calculations) — Geography Grade 12", "Map Work (Advanced Calculations)", 3, new Guid("7a475323-7df5-f613-6449-fcfad4461d7c") },
                    { new Guid("fe526bf2-961d-f568-89bf-28c52fc115a2"), "Analytical Geometry (Circles) — Mathematics Grade 11", "Analytical Geometry (Circles)", 7, new Guid("5295e6aa-8238-0794-a9ec-29deb0bef2aa") },
                    { new Guid("01b83c0a-da1b-d3ee-63c6-a7223a562a70"), "Optical Phenomena — Science Grade 12", "Optical Phenomena", 7, new Guid("89bdf99e-82a8-29b3-57d7-410b42d8dcf8") },
                    { new Guid("01c89125-6054-4ff5-732e-e588e2723bd7"), "Ideal Gases — Science Grade 11", "Ideal Gases", 7, new Guid("82786b28-5a6d-b0b6-194d-ab131764064d") },
                    { new Guid("0418668d-5ea4-5b7c-9fa6-1f46a2b4ae82"), "Summary Writing — English Grade 10", "Summary Writing", 2, new Guid("fdca9ad9-bc36-79d0-421e-38c118a6c3cf") },
                    { new Guid("0542c6bb-6c8c-3d9a-e9fc-af56459051ed"), "Quantitative Aspects of Chemical Change — Science Grade 11", "Quantitative Aspects of Chemical Change", 8, new Guid("82786b28-5a6d-b0b6-194d-ab131764064d") },
                    { new Guid("0651b6ce-8c9c-a631-9a5c-832a62d3f4ce"), "Vectors and Scalars — Science Grade 11", "Vectors and Scalars", 1, new Guid("82786b28-5a6d-b0b6-194d-ab131764064d") },
                    { new Guid("09e08e66-3440-ad62-4e19-33609a335577"), "Literature: Poetry Analysis — English Grade 11", "Literature: Poetry Analysis", 4, new Guid("99b68af6-34fc-713c-884f-c018c6b17d72") },
                    { new Guid("0a968297-5849-7e33-62a7-42614e560e53"), "Literature: Drama — English Grade 10", "Literature: Drama", 6, new Guid("fdca9ad9-bc36-79d0-421e-38c118a6c3cf") },
                    { new Guid("0e351883-931d-6518-94a2-8bfa033b954c"), "Momentum and Impulse — Science Grade 12", "Momentum and Impulse", 1, new Guid("89bdf99e-82a8-29b3-57d7-410b42d8dcf8") },
                    { new Guid("14ce031f-6c0a-6af2-279f-4b6e66c27169"), "Transactional Writing (Speech, Interview) — English Grade 12", "Transactional Writing (Speech, Interview)", 7, new Guid("532a8048-546e-3d68-aa5a-14ae71eb6f9e") },
                    { new Guid("297b9215-1ec3-f632-2c1a-7e790fcdfb57"), "Mechanical Energy — Science Grade 11", "Mechanical Energy", 3, new Guid("82786b28-5a6d-b0b6-194d-ab131764064d") },
                    { new Guid("2f5c42c7-5b66-043f-a338-ff1489cb84b4"), "Transactional Writing (Formal Letters, Reviews) — English Grade 11", "Transactional Writing (Formal Letters, Reviews)", 8, new Guid("99b68af6-34fc-713c-884f-c018c6b17d72") },
                    { new Guid("2ff2ff99-92d5-5e48-22cd-999d6bcdbe81"), "Literature: Novel Study — English Grade 11", "Literature: Novel Study", 3, new Guid("99b68af6-34fc-713c-884f-c018c6b17d72") },
                    { new Guid("3284ed1c-bfd2-e1b4-39ef-b396f35857ad"), "Comprehension and Language — English Grade 11", "Comprehension and Language", 1, new Guid("99b68af6-34fc-713c-884f-c018c6b17d72") },
                    { new Guid("35c791a5-93f6-d9c6-fba4-2813a98e1c27"), "Doppler Effect — Science Grade 12", "Doppler Effect", 5, new Guid("89bdf99e-82a8-29b3-57d7-410b42d8dcf8") },
                    { new Guid("3cd5f9e7-e4d7-93ad-6b57-8960015fda54"), "Scientific Method and Skills — Science Grade 10", "Scientific Method and Skills", 1, new Guid("082d128a-6ee4-2c5d-0efc-f9b0b9af4c23") },
                    { new Guid("3e7dd8ba-27be-47e9-7819-37c77520a90d"), "Exam Preparation and Techniques — English Grade 12", "Exam Preparation and Techniques", 10, new Guid("532a8048-546e-3d68-aa5a-14ae71eb6f9e") },
                    { new Guid("46a1de31-acbc-fa47-f1d9-a4c405ad4c06"), "Electrodynamics — Science Grade 12", "Electrodynamics", 6, new Guid("89bdf99e-82a8-29b3-57d7-410b42d8dcf8") },
                    { new Guid("49274ac5-49e2-98a0-0f81-21ed00496707"), "Comprehension and Language — English Grade 12", "Comprehension and Language", 1, new Guid("532a8048-546e-3d68-aa5a-14ae71eb6f9e") },
                    { new Guid("4c9b3c0e-ea03-6dc1-edca-b11ae4d2d37d"), "Electromagnetic Radiation — Science Grade 10", "Electromagnetic Radiation", 7, new Guid("082d128a-6ee4-2c5d-0efc-f9b0b9af4c23") },
                    { new Guid("54cebaf0-c5b2-446b-017d-a3b10deb3679"), "Visual Literacy (Cartoons, Advertisements) — English Grade 12", "Visual Literacy (Cartoons, Advertisements)", 8, new Guid("532a8048-546e-3d68-aa5a-14ae71eb6f9e") },
                    { new Guid("639bf567-8933-910e-f99e-3033e568d205"), "Intermolecular Forces — Science Grade 11", "Intermolecular Forces", 6, new Guid("82786b28-5a6d-b0b6-194d-ab131764064d") },
                    { new Guid("6c6e3005-6ee3-bbe6-d150-eb76b67a30ca"), "Electrochemistry — Science Grade 12", "Electrochemistry", 8, new Guid("89bdf99e-82a8-29b3-57d7-410b42d8dcf8") },
                    { new Guid("7571f553-46da-20c5-75db-9943be088f12"), "Literature: Poetry (Exam Prep) — English Grade 12", "Literature: Poetry (Exam Prep)", 4, new Guid("532a8048-546e-3d68-aa5a-14ae71eb6f9e") },
                    { new Guid("832be98d-ff03-a21e-b1ec-34e30faca608"), "The Atom — Science Grade 10", "The Atom", 4, new Guid("082d128a-6ee4-2c5d-0efc-f9b0b9af4c23") },
                    { new Guid("837f11aa-9d94-bd5c-6db6-fb2e8ec2c506"), "Electric Circuits (Series and Parallel) — Science Grade 11", "Electric Circuits (Series and Parallel)", 10, new Guid("82786b28-5a6d-b0b6-194d-ab131764064d") },
                    { new Guid("91a31b80-6aaa-df03-65d7-616e68adcbb9"), "Newton's Laws of Motion — Science Grade 11", "Newton's Laws of Motion", 2, new Guid("82786b28-5a6d-b0b6-194d-ab131764064d") },
                    { new Guid("92bfd75c-ad93-0d18-c3bf-1dbfc0062da0"), "Electric Circuits — Science Grade 10", "Electric Circuits", 10, new Guid("082d128a-6ee4-2c5d-0efc-f9b0b9af4c23") },
                    { new Guid("92c8e20b-a633-3a98-a238-b618bc716f8b"), "Acids and Bases — Science Grade 12", "Acids and Bases", 10, new Guid("89bdf99e-82a8-29b3-57d7-410b42d8dcf8") },
                    { new Guid("9353cb3e-0beb-f23b-c2f4-214538abf8be"), "Creative Writing (Narrative, Descriptive) — English Grade 10", "Creative Writing (Narrative, Descriptive)", 7, new Guid("fdca9ad9-bc36-79d0-421e-38c118a6c3cf") },
                    { new Guid("988c83c1-55da-d3e2-12aa-de74d132c448"), "Vertical Projectile Motion — Science Grade 12", "Vertical Projectile Motion", 2, new Guid("89bdf99e-82a8-29b3-57d7-410b42d8dcf8") },
                    { new Guid("9d6e7e68-fc31-da6c-7467-2adbc7ee1638"), "Grammar, Editing and Language Structures — English Grade 11", "Grammar, Editing and Language Structures", 10, new Guid("99b68af6-34fc-713c-884f-c018c6b17d72") },
                    { new Guid("9d81c63d-ced0-fb8a-0dec-22f293b69ae4"), "Visual Literacy and Advertising — English Grade 11", "Visual Literacy and Advertising", 9, new Guid("99b68af6-34fc-713c-884f-c018c6b17d72") },
                    { new Guid("9df9e604-aa11-b7f5-eefa-56adc54d807c"), "Creative Writing (Argumentative, Reflective) — English Grade 12", "Creative Writing (Argumentative, Reflective)", 6, new Guid("532a8048-546e-3d68-aa5a-14ae71eb6f9e") },
                    { new Guid("a10948f2-bb20-75fa-7829-1d374f32ff72"), "Summary Writing — English Grade 11", "Summary Writing", 2, new Guid("99b68af6-34fc-713c-884f-c018c6b17d72") },
                    { new Guid("a1ec96b8-4bf8-6b02-4a31-694e5157a900"), "Electrostatics (Electric Fields) — Science Grade 11", "Electrostatics (Electric Fields)", 9, new Guid("82786b28-5a6d-b0b6-194d-ab131764064d") },
                    { new Guid("a8039e66-9ee4-9b91-0790-b563c3701ded"), "Literature: Drama (Exam Prep) — English Grade 12", "Literature: Drama (Exam Prep)", 5, new Guid("532a8048-546e-3d68-aa5a-14ae71eb6f9e") },
                    { new Guid("abaa3fa4-c13c-5e90-6fda-f781cd3c883a"), "Creative Writing (Essays, Narratives) — English Grade 11", "Creative Writing (Essays, Narratives)", 7, new Guid("99b68af6-34fc-713c-884f-c018c6b17d72") },
                    { new Guid("acd2944d-8102-2c04-b518-e0520dc91d6b"), "Electrostatics — Science Grade 10", "Electrostatics", 9, new Guid("082d128a-6ee4-2c5d-0efc-f9b0b9af4c23") },
                    { new Guid("b03e4dd7-c5ee-6122-294d-5cf2c6543156"), "Chemical Bonding — Science Grade 10", "Chemical Bonding", 5, new Guid("082d128a-6ee4-2c5d-0efc-f9b0b9af4c23") },
                    { new Guid("b0b02cb0-f0eb-a064-ee36-75ea48b10fe5"), "Waves, Sound and Light — Science Grade 11", "Waves, Sound and Light", 4, new Guid("82786b28-5a6d-b0b6-194d-ab131764064d") },
                    { new Guid("b189674c-4e35-7dae-e043-007edf702650"), "Literature: Novel Study — English Grade 10", "Literature: Novel Study", 3, new Guid("fdca9ad9-bc36-79d0-421e-38c118a6c3cf") },
                    { new Guid("b25ad283-d4a3-a8e3-e2a3-3b2680f3be1e"), "Transverse Waves — Science Grade 10", "Transverse Waves", 6, new Guid("082d128a-6ee4-2c5d-0efc-f9b0b9af4c23") },
                    { new Guid("b578853a-5577-6231-4ead-4700bcdbe6fd"), "Comprehension and Language — English Grade 10", "Comprehension and Language", 1, new Guid("fdca9ad9-bc36-79d0-421e-38c118a6c3cf") },
                    { new Guid("c482fee0-eaa6-b70d-b94e-19495a18b0c8"), "Literature: Short Stories Analysis — English Grade 11", "Literature: Short Stories Analysis", 5, new Guid("99b68af6-34fc-713c-884f-c018c6b17d72") },
                    { new Guid("c637c9c3-e71e-ef19-b93c-038e1060c339"), "Summary Writing — English Grade 12", "Summary Writing", 2, new Guid("532a8048-546e-3d68-aa5a-14ae71eb6f9e") },
                    { new Guid("c8b0d99d-86a7-208f-43a9-f94003608d3b"), "Literature: Novel Study (Exam Prep) — English Grade 12", "Literature: Novel Study (Exam Prep)", 3, new Guid("532a8048-546e-3d68-aa5a-14ae71eb6f9e") },
                    { new Guid("c8cfe728-fe0a-0883-ad14-0057aef8697e"), "Work, Energy and Power — Science Grade 12", "Work, Energy and Power", 4, new Guid("89bdf99e-82a8-29b3-57d7-410b42d8dcf8") },
                    { new Guid("cdd08660-6404-dd4b-4da7-f58d68ae3897"), "Literature: Drama Analysis — English Grade 11", "Literature: Drama Analysis", 6, new Guid("99b68af6-34fc-713c-884f-c018c6b17d72") },
                    { new Guid("d02c0f9a-e3f0-499e-744f-1555b82d3581"), "Literature: Poetry — English Grade 10", "Literature: Poetry", 4, new Guid("fdca9ad9-bc36-79d0-421e-38c118a6c3cf") },
                    { new Guid("d577391d-8052-ee84-1167-f7a74473a137"), "Grammar and Sentence Structure — English Grade 10", "Grammar and Sentence Structure", 10, new Guid("fdca9ad9-bc36-79d0-421e-38c118a6c3cf") },
                    { new Guid("df945f07-f6fd-d786-9e3a-ca853df8eddf"), "States of Matter and Energy — Science Grade 10", "States of Matter and Energy", 3, new Guid("082d128a-6ee4-2c5d-0efc-f9b0b9af4c23") },
                    { new Guid("e0c6257b-a5de-6c40-b726-1aceac8e2f9a"), "Visual Literacy — English Grade 10", "Visual Literacy", 9, new Guid("fdca9ad9-bc36-79d0-421e-38c118a6c3cf") },
                    { new Guid("e88283a1-61e0-ee45-46c6-693bdca5ee44"), "Grammar and Editing — English Grade 12", "Grammar and Editing", 9, new Guid("532a8048-546e-3d68-aa5a-14ae71eb6f9e") },
                    { new Guid("e951ea1c-8c9d-cc2b-6b45-c0916476bbd0"), "Chemical Change (Stoichiometry) — Science Grade 11", "Chemical Change (Stoichiometry)", 5, new Guid("82786b28-5a6d-b0b6-194d-ab131764064d") },
                    { new Guid("ecda0c05-a8b9-5244-b3dd-743ebfb367b0"), "Magnetism — Science Grade 10", "Magnetism", 8, new Guid("082d128a-6ee4-2c5d-0efc-f9b0b9af4c23") },
                    { new Guid("f1499460-3482-edd8-102f-374954f392ce"), "Matter and Classification — Science Grade 10", "Matter and Classification", 2, new Guid("082d128a-6ee4-2c5d-0efc-f9b0b9af4c23") },
                    { new Guid("f2171eb6-9c3d-271d-9302-bd5d1bf36cdb"), "Literature: Short Stories — English Grade 10", "Literature: Short Stories", 5, new Guid("fdca9ad9-bc36-79d0-421e-38c118a6c3cf") },
                    { new Guid("f42371cf-aaf2-cfa0-994c-cc134d4fa9ad"), "Chemical Equilibrium — Science Grade 12", "Chemical Equilibrium", 9, new Guid("89bdf99e-82a8-29b3-57d7-410b42d8dcf8") },
                    { new Guid("f7e8d9d9-e003-667e-a79b-867704c18363"), "Transactional Writing (Letters, Reports) — English Grade 10", "Transactional Writing (Letters, Reports)", 8, new Guid("fdca9ad9-bc36-79d0-421e-38c118a6c3cf") },
                    { new Guid("f9f3697d-74dc-5cac-eaf3-9dc2d7c4e5f9"), "Organic Chemistry — Science Grade 12", "Organic Chemistry", 3, new Guid("89bdf99e-82a8-29b3-57d7-410b42d8dcf8") }
                });
        }
    }
}
