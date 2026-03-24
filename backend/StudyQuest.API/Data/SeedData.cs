using Microsoft.EntityFrameworkCore;
using StudyQuest.API.Models;

namespace StudyQuest.API.Data;

public static class SeedData
{
    public static void Seed(ModelBuilder modelBuilder)
    {
        var subjects = new List<Subject>();
        var topics = new List<Topic>();

        // Define subjects and their topics per grade
        var curriculum = GetCurriculum();

        foreach (var (subjectName, color, gradeTopics) in curriculum)
        {
            foreach (var (grade, topicNames) in gradeTopics)
            {
                var subjectId = GenerateGuid($"{subjectName}-{grade}");
                subjects.Add(new Subject
                {
                    Id = subjectId,
                    Name = subjectName,
                    Grade = grade,
                    Description = $"{subjectName} — WASSCE (Grade {grade})",
                    Color = color
                });

                for (int i = 0; i < topicNames.Length; i++)
                {
                    topics.Add(new Topic
                    {
                        Id = GenerateGuid($"{subjectName}-{grade}-{topicNames[i]}"),
                        SubjectId = subjectId,
                        Name = topicNames[i],
                        Order = i + 1,
                        Description = $"{topicNames[i]} — {subjectName} Grade {grade}"
                    });
                }
            }
        }

        modelBuilder.Entity<Subject>().HasData(subjects);
        modelBuilder.Entity<Topic>().HasData(topics);
    }

    private static Guid GenerateGuid(string seed)
    {
        using var md5 = System.Security.Cryptography.MD5.Create();
        var hash = md5.ComputeHash(System.Text.Encoding.UTF8.GetBytes(seed));
        return new Guid(hash);
    }

    private static List<(string Name, string Color, List<(int Grade, string[] Topics)> GradeTopics)> GetCurriculum()
    {
        return
        [
            // ── 1. Mathematics (Core) ──────────────────────────────────────
            ("Mathematics", "#ef4444", [
                (10, [
                    "Number and Numeration",
                    "Fractions, Decimals and Approximations",
                    "Indices and Logarithms",
                    "Algebraic Expressions and Factorisation",
                    "Simple Equations and Inequalities",
                    "Geometry (Angles, Triangles, Polygons)",
                    "Mensuration (Perimeter, Area, Volume)",
                    "Basic Trigonometry (Sine, Cosine, Tangent)",
                    "Statistics (Data Collection, Mean, Median, Mode)",
                    "Probability (Basic Concepts)"
                ]),
                (11, [
                    "Sets and Venn Diagrams",
                    "Quadratic Equations",
                    "Simultaneous Equations",
                    "Variation (Direct, Inverse, Joint, Partial)",
                    "Functions and Relations",
                    "Coordinate Geometry (Straight Lines)",
                    "Trigonometry (Graphs, Identities, Equations)",
                    "Mensuration (Circles, Sectors, Arcs)",
                    "Statistics (Frequency Distributions, Histograms)",
                    "Probability (Combined Events)"
                ]),
                (12, [
                    "Sequences and Series (AP, GP)",
                    "Matrices and Determinants",
                    "Vectors in Two Dimensions",
                    "Transformation Geometry",
                    "Trigonometry (Sine and Cosine Rules)",
                    "Circle Theorems",
                    "Construction and Loci",
                    "Mensuration (Cones, Spheres, Pyramids)",
                    "Statistics (Cumulative Frequency, Quartiles)",
                    "Probability (Permutations, Combinations)",
                    "WASSCE Past Questions Practice"
                ])
            ]),

            // ── 2. English Language ────────────────────────────────────────
            ("English Language", "#3b82f6", [
                (10, [
                    "Comprehension (Passages and Inference)",
                    "Summary Writing",
                    "Lexis and Structure (Vocabulary Building)",
                    "Parts of Speech and Sentence Structure",
                    "Tenses and Concord",
                    "Narrative and Descriptive Essay Writing",
                    "Letter Writing (Informal)",
                    "Oral English (Vowel and Consonant Sounds)",
                    "Reading Skills and Techniques",
                    "Punctuation and Spelling"
                ]),
                (11, [
                    "Comprehension (Advanced Inference and Deduction)",
                    "Summary Writing (Advanced Techniques)",
                    "Lexis and Structure (Idioms, Phrasal Verbs)",
                    "Clause Analysis and Complex Sentences",
                    "Argumentative and Expository Essay Writing",
                    "Formal Letter Writing and Reports",
                    "Oral English (Stress and Intonation Patterns)",
                    "Speech Writing and Debate",
                    "Register and Style",
                    "Common Errors in English Usage"
                ]),
                (12, [
                    "Comprehension (WASSCE Paper 1 Practice)",
                    "Summary Writing (WASSCE Paper 2 Practice)",
                    "Lexis and Structure (Exam-Level Vocabulary)",
                    "Essay Writing (All Types — WASSCE Format)",
                    "Letter Writing (Formal, Semi-Formal, Informal)",
                    "Article, Report and Speech Writing",
                    "Oral English (WASSCE Listening Test Prep)",
                    "Grammar Revision (Exam Focus)",
                    "Literary Appreciation (Prose, Poetry, Drama)",
                    "WASSCE Past Questions and Exam Strategies"
                ])
            ]),

            // ── 3. Physics ─────────────────────────────────────────────────
            ("Physics", "#a855f7", [
                (10, [
                    "Measurements and Units",
                    "Scalars and Vectors",
                    "Motion (Speed, Velocity, Acceleration)",
                    "Causes of Motion (Newton's Laws)",
                    "Work, Energy and Power",
                    "Pressure (Solids, Liquids, Gases)",
                    "Thermal Energy and Temperature",
                    "Heat Transfer (Conduction, Convection, Radiation)",
                    "Waves (Types and Properties)",
                    "Sound Waves"
                ]),
                (11, [
                    "Linear Momentum and Collisions",
                    "Equilibrium of Forces (Moments, Couples)",
                    "Machines (Levers, Pulleys, Inclined Planes)",
                    "Elasticity (Hooke's Law)",
                    "Fluids at Rest and in Motion",
                    "Light (Reflection, Refraction, Lenses)",
                    "Optical Instruments",
                    "Electrostatics (Charges, Electric Fields)",
                    "Capacitance and Capacitors",
                    "Current Electricity (Ohm's Law, Circuits)"
                ]),
                (12, [
                    "Electromagnetic Induction (Faraday's Law)",
                    "Alternating Current Circuits",
                    "Magnetic Fields and Forces",
                    "Electric Cells and Energy",
                    "Introductory Electronics (Diodes, Transistors)",
                    "Atomic and Nuclear Physics",
                    "Radioactivity and Half-Life",
                    "Wave-Particle Duality",
                    "Electromagnetic Spectrum",
                    "WASSCE Practical Physics Skills",
                    "WASSCE Past Questions Practice"
                ])
            ]),

            // ── 4. Biology ─────────────────────────────────────────────────
            ("Biology", "#ec4899", [
                (10, [
                    "Recognising Living Things",
                    "Cell Structure and Organisation",
                    "Cell Division (Mitosis)",
                    "Classification of Living Things",
                    "Nutrition in Plants (Photosynthesis)",
                    "Nutrition in Animals",
                    "Transport in Living Things",
                    "Respiratory System",
                    "Excretion",
                    "Factors Affecting Living Organisms"
                ]),
                (11, [
                    "Ecology (Habitats, Biomes, Ecosystems)",
                    "Energy Flow and Nutrient Cycling",
                    "Reproductive Systems in Plants",
                    "Reproductive Systems in Animals",
                    "Growth and Development",
                    "Coordination and Control (Nervous System)",
                    "Hormonal Coordination (Endocrine System)",
                    "Sense Organs (Eye, Ear)",
                    "Skeletal and Muscular Systems",
                    "Diseases and Immunology"
                ]),
                (12, [
                    "Genetics (Mendel's Laws, Inheritance Patterns)",
                    "Variation and Evolution",
                    "Meiosis and Genetic Variation",
                    "Ecology and Conservation of Natural Resources",
                    "Pollution and Its Effects",
                    "Adaptation of Organisms",
                    "DNA, Genes and Chromosomes",
                    "Biotechnology and Genetic Engineering",
                    "Biology and Human Welfare",
                    "WASSCE Practical Biology Skills",
                    "WASSCE Past Questions Practice"
                ])
            ]),

            // ── 5. Chemistry ───────────────────────────────────────────────
            ("Chemistry", "#f59e0b", [
                (10, [
                    "Introduction to Chemistry (Laboratory Safety)",
                    "Particulate Nature of Matter",
                    "Separation Techniques",
                    "Atomic Structure and the Periodic Table",
                    "Chemical Bonding (Ionic, Covalent, Metallic)",
                    "Chemical Formulae and Equations",
                    "States of Matter and Gas Laws",
                    "Water and Solutions",
                    "Acids, Bases and Salts (Introduction)",
                    "Chemical Reactions (Types)"
                ]),
                (11, [
                    "The Mole Concept and Stoichiometry",
                    "Electrolysis and Electrochemistry",
                    "Energy Changes in Reactions (Enthalpy)",
                    "Rates of Chemical Reactions",
                    "Chemical Equilibrium",
                    "Acids, Bases and Salts (Advanced)",
                    "Carbon and Its Compounds (Organic Chemistry Intro)",
                    "Metals and Their Compounds",
                    "Non-Metals and Their Compounds",
                    "Qualitative Analysis"
                ]),
                (12, [
                    "Organic Chemistry (Hydrocarbons)",
                    "Organic Chemistry (Functional Groups)",
                    "Organic Chemistry (Polymers and Plastics)",
                    "Industrial Chemistry (Extraction of Metals)",
                    "Environmental Chemistry (Pollution)",
                    "Nuclear Chemistry",
                    "Redox Reactions and IUPAC Nomenclature",
                    "Quantitative Analysis (Titrations)",
                    "WASSCE Practical Chemistry Skills",
                    "WASSCE Past Questions Practice"
                ])
            ]),

            // ── 6. Agriculture ─────────────────────────────────────────────
            ("Agriculture", "#84cc16", [
                (10, [
                    "Meaning and Importance of Agriculture",
                    "Types of Agriculture in The Gambia",
                    "Soil Science (Formation, Types, Profiles)",
                    "Soil Fertility and Conservation",
                    "Farm Tools and Implements",
                    "Plant Morphology and Physiology",
                    "Crop Production (Land Preparation, Planting)",
                    "Irrigation and Water Management",
                    "Pest and Disease Control in Crops",
                    "Introduction to Animal Husbandry"
                ]),
                (11, [
                    "Crop Improvement and Seed Technology",
                    "Weed Management",
                    "Crop Processing and Storage",
                    "Animal Nutrition and Feeding",
                    "Livestock Management (Cattle, Poultry, Small Ruminants)",
                    "Animal Health and Disease Control",
                    "Agricultural Economics (Demand, Supply, Pricing)",
                    "Farm Records and Accounts",
                    "Agricultural Extension Services",
                    "Forestry and Agroforestry"
                ]),
                (12, [
                    "Animal Reproduction and Breeding",
                    "Pasture and Forage Crops",
                    "Agricultural Mechanisation",
                    "Agricultural Marketing and Trade",
                    "Farm Management and Business Planning",
                    "Agricultural Credit and Cooperatives",
                    "Environmental Impact of Farming",
                    "Food Security and Safety in West Africa",
                    "Government Agricultural Policies",
                    "WASSCE Past Questions Practice"
                ])
            ]),

            // ── 7. Geography ───────────────────────────────────────────────
            ("Geography", "#06b6d4", [
                (10, [
                    "The Earth and the Solar System",
                    "Map Reading (Scales, Symbols, Bearings)",
                    "Landforms and Drainage",
                    "Rocks and Minerals",
                    "Weathering and Erosion",
                    "Climate and Vegetation of West Africa",
                    "Population (Distribution, Density, Growth)",
                    "Settlement Types and Patterns",
                    "Agriculture in The Gambia and West Africa",
                    "Environmental Problems"
                ]),
                (11, [
                    "Map Reading (Contours, Cross-Sections)",
                    "Weather and Climate (Elements and Instruments)",
                    "Geomorphic Processes (Rivers, Coasts, Wind)",
                    "Vegetation Zones of Africa",
                    "Population (Migration, Urbanisation)",
                    "Transportation and Communication",
                    "Manufacturing Industries",
                    "Mining and Power Resources",
                    "Trade (Internal and International)",
                    "Tourism in The Gambia and West Africa"
                ]),
                (12, [
                    "Map Reading (Advanced Interpretation)",
                    "Physical Geography of Africa and the World",
                    "Economic Activities of Major World Regions",
                    "Regional Geography of West Africa",
                    "The Gambia — Physical and Economic Geography",
                    "Environmental Conservation and Management",
                    "Natural Hazards and Disasters",
                    "Globalisation and Development",
                    "GIS and Remote Sensing (Introduction)",
                    "WASSCE Map Work and Past Questions Practice"
                ])
            ]),

            // ── 8. Economics ───────────────────────────────────────────────
            ("Economics", "#10b981", [
                (10, [
                    "Basic Economic Concepts (Scarcity, Choice, Opportunity Cost)",
                    "Economic Systems (Market, Command, Mixed)",
                    "Demand, Supply and Price Determination",
                    "Theory of Consumer Behaviour",
                    "Theory of Production",
                    "Factors of Production",
                    "Business Organisations (Sole Trader, Partnerships, Companies)",
                    "Population and Labour Force",
                    "Introduction to Money and Banking",
                    "The Gambian Economy (Overview)"
                ]),
                (11, [
                    "Market Structures (Perfect Competition, Monopoly, Oligopoly)",
                    "National Income (Measurement and Concepts)",
                    "Money, Banking and Financial Institutions",
                    "Inflation and Deflation",
                    "Fiscal and Monetary Policy",
                    "Public Finance (Government Revenue and Expenditure)",
                    "Agriculture and Industry in West Africa",
                    "Distributive Trade",
                    "Transportation and Communication",
                    "Balance of Payments"
                ]),
                (12, [
                    "International Trade (Theory, Barriers, Agreements)",
                    "Economic Integration (ECOWAS, AU)",
                    "Economic Development and Planning",
                    "Petroleum and the Nigerian Economy",
                    "Economic Problems of West Africa",
                    "Globalisation and Economic Growth",
                    "Unemployment and Poverty Alleviation",
                    "Privatisation and Commercialisation",
                    "Environmental Economics",
                    "WASSCE Past Questions Practice"
                ])
            ]),

            // ── 9. Government ──────────────────────────────────────────────
            ("Government", "#6366f1", [
                (10, [
                    "Basic Concepts in Government (State, Power, Authority)",
                    "Forms of Government (Democracy, Monarchy, Oligarchy)",
                    "Arms of Government (Legislature, Executive, Judiciary)",
                    "Principles of Democracy",
                    "Political Participation and Citizenship",
                    "The Constitution (Types, Features, Importance)",
                    "Fundamental Human Rights",
                    "Rule of Law and Constitutionalism",
                    "Political Parties and Pressure Groups",
                    "Elections and Electoral Systems"
                ]),
                (11, [
                    "Local Government Administration",
                    "Public Administration and Civil Service",
                    "Political Culture and Socialisation",
                    "The Military in Politics",
                    "Federalism and Unitarism",
                    "Pre-Colonial Political Systems in West Africa",
                    "Colonial Administration in West Africa",
                    "Nationalism and Independence Movements",
                    "Government of The Gambia (Structure and Functions)",
                    "Decentralisation and Devolution"
                ]),
                (12, [
                    "Government of Nigeria (Comparison)",
                    "Government of Ghana (Comparison)",
                    "International Organisations (UN, AU, ECOWAS, Commonwealth)",
                    "Non-Governmental Organisations (NGOs)",
                    "Conflicts, Resolution and Peace-building",
                    "Human Rights and the Rule of Law in Practice",
                    "Challenges of Governance in West Africa",
                    "Corruption and Anti-Corruption Measures",
                    "Contemporary Political Issues",
                    "WASSCE Past Questions Practice"
                ])
            ]),

            // ── 10. Commerce / Business Studies ────────────────────────────
            ("Commerce", "#f97316", [
                (10, [
                    "Introduction to Commerce and Trade",
                    "Home Trade (Wholesale and Retail)",
                    "Foreign Trade (Imports, Exports, Entreports)",
                    "Aids to Trade (Overview)",
                    "Money (Functions, Qualities, Types)",
                    "Banking (Types of Banks, Services)",
                    "Insurance (Principles, Types of Policies)",
                    "Transportation (Modes, Importance)",
                    "Communication in Business",
                    "Business Organisations (Types and Formation)"
                ]),
                (11, [
                    "The Stock Exchange and Capital Market",
                    "Marketing (Concepts, Functions, Mix)",
                    "Advertising and Sales Promotion",
                    "Warehousing and Storage",
                    "Tourism and Hospitality",
                    "Consumer Protection",
                    "Business Finance (Sources of Capital)",
                    "Trade Associations and Chambers of Commerce",
                    "Government and Business Regulation",
                    "Information and Communication Technology in Business"
                ]),
                (12, [
                    "International Trade Documents and Procedures",
                    "Balance of Trade and Balance of Payments",
                    "Economic Integration and Trade Blocs",
                    "E-Commerce and Digital Business",
                    "Business Ethics and Social Responsibility",
                    "Entrepreneurship and Small Business Management",
                    "Globalisation and International Business",
                    "Public Enterprises and Privatisation",
                    "Commercial Law (Contracts, Agency)",
                    "WASSCE Past Questions Practice"
                ])
            ]),

            // ── 11. Financial Accounting ───────────────────────────────────
            ("Financial Accounting", "#14b8a6", [
                (10, [
                    "Introduction to Book-Keeping and Accounting",
                    "Source Documents and Books of Original Entry",
                    "Double Entry System",
                    "The Ledger (Personal, Real, Nominal Accounts)",
                    "Cash Book (Single, Double, Three-Column)",
                    "Trial Balance",
                    "Trading, Profit and Loss Account",
                    "Balance Sheet",
                    "Bank Reconciliation Statement",
                    "Correction of Errors"
                ]),
                (11, [
                    "Control Accounts (Sales and Purchases Ledger)",
                    "Depreciation of Fixed Assets",
                    "Provision for Bad and Doubtful Debts",
                    "Accounts of Non-Profit Organisations",
                    "Manufacturing Accounts",
                    "Single Entry and Incomplete Records",
                    "Partnership Accounts (Formation, Profit Sharing)",
                    "Dissolution of Partnership",
                    "Accounting Ratios and Interpretation",
                    "Departmental Accounts"
                ]),
                (12, [
                    "Company Accounts (Shares, Debentures)",
                    "Published Accounts and Financial Statements",
                    "Stock Valuation (FIFO, LIFO, AVCO)",
                    "Accounts from Incomplete Records (Advanced)",
                    "Branch Accounts",
                    "Hire Purchase and Instalment Accounts",
                    "Royalty Accounts",
                    "Computerised Accounting Systems",
                    "Auditing Basics",
                    "WASSCE Past Questions Practice"
                ])
            ]),

            // ── 12. History ────────────────────────────────────────────────
            ("History", "#8b5cf6", [
                (10, [
                    "Sources and Methods of Studying History",
                    "Early Civilisations (Egypt, Mali, Songhai)",
                    "Pre-Colonial Societies in West Africa",
                    "The Trans-Saharan Trade",
                    "The Atlantic Slave Trade",
                    "Islamic Influence in West Africa",
                    "European Exploration and Contact",
                    "The Scramble for and Partition of Africa",
                    "The Gambia Before Colonial Rule",
                    "Overview of West African Empires"
                ]),
                (11, [
                    "Colonial Administration (British, French, Portuguese)",
                    "Colonial Economy and Social Change",
                    "Resistance to Colonial Rule",
                    "Nationalism in West Africa",
                    "Road to Independence (The Gambia, Ghana, Nigeria)",
                    "World War I and II (Impact on Africa)",
                    "Pan-Africanism and the OAU/AU",
                    "The Gambian Independence Movement",
                    "Post-Independence Challenges",
                    "Military Interventions in West Africa"
                ]),
                (12, [
                    "Post-Independence Political Development in West Africa",
                    "The Gambia: First and Second Republics",
                    "ECOWAS and Regional Cooperation",
                    "Apartheid and Liberation in Southern Africa",
                    "The United Nations and Africa",
                    "The Cold War and Its Impact on Africa",
                    "Conflict and Conflict Resolution in West Africa",
                    "Economic Development Since Independence",
                    "Contemporary Issues in African History",
                    "WASSCE Past Questions Practice"
                ])
            ]),

            // ── 13. Christian Religious Studies (CRS) ──────────────────────
            ("Christian Religious Studies", "#f43f5e", [
                (10, [
                    "Creation and the Fall of Man",
                    "The Patriarchs (Abraham, Isaac, Jacob, Joseph)",
                    "Moses and the Exodus",
                    "The Ten Commandments and the Covenant",
                    "The Judges of Israel",
                    "The United Monarchy (Saul, David, Solomon)",
                    "The Divided Kingdom and the Prophets",
                    "Selected Psalms and Proverbs",
                    "Introduction to the New Testament",
                    "The Birth and Early Life of Jesus"
                ]),
                (11, [
                    "The Ministry of Jesus (Teachings and Parables)",
                    "The Miracles of Jesus",
                    "The Sermon on the Mount",
                    "The Passion, Death and Resurrection of Jesus",
                    "The Acts of the Apostles (Early Church)",
                    "The Missionary Journeys of Paul",
                    "The Epistles (Pauline and General Letters)",
                    "Christian Ethics (Love, Justice, Forgiveness)",
                    "Marriage and Family Life",
                    "Christians and the State"
                ]),
                (12, [
                    "The Church (Nature, Unity, Mission)",
                    "Christianity and African Traditional Religion",
                    "Faith and Works (James, Hebrews)",
                    "The Holy Spirit and Christian Living",
                    "Social Issues (Poverty, Corruption, Drug Abuse)",
                    "Conflict Resolution and Peace",
                    "Leadership and Service in the Church",
                    "Christianity and Science",
                    "Eschatology (Last Things, Revelation)",
                    "WASSCE Past Questions Practice"
                ])
            ]),

            // ── 14. Islamic Religious Studies (IRS) ────────────────────────
            ("Islamic Religious Studies", "#eab308", [
                (10, [
                    "Tawhid (Oneness of Allah)",
                    "The Quran (Revelation, Compilation, Structure)",
                    "Selected Surahs and Their Meanings",
                    "The Five Pillars of Islam",
                    "The Six Articles of Faith (Iman)",
                    "The Life of Prophet Muhammad (Makkan Period)",
                    "Hadith (Introduction, Classification)",
                    "Purification and Salat (Prayer)",
                    "Islamic Morals and Manners (Akhlaq)",
                    "Introduction to Fiqh (Islamic Jurisprudence)"
                ]),
                (11, [
                    "The Life of Prophet Muhammad (Madinan Period)",
                    "The Rightly Guided Caliphs (Abu Bakr, Umar, Uthman, Ali)",
                    "Zakat, Sawm and Hajj (In Detail)",
                    "The Quran and Science",
                    "Islamic Family Law (Marriage, Divorce, Inheritance)",
                    "Islamic Economic System",
                    "Hadith Studies (Selected Ahadith)",
                    "Islam in West Africa (History and Spread)",
                    "Islamic Contributions to Civilisation",
                    "Moral Teachings in Islam"
                ]),
                (12, [
                    "The Umayyad and Abbasid Dynasties",
                    "Islamic Movements in West Africa (Jihads)",
                    "Islam and Contemporary Issues",
                    "Islamic Criminal Law and Justice",
                    "Comparative Religion (Islam, Christianity, ATR)",
                    "Tafsir (Quranic Exegesis) of Selected Passages",
                    "Islamic Political System (Shura, Caliphate)",
                    "Islam and Science, Technology and Development",
                    "Muslim Scholars and Their Contributions",
                    "WASSCE Past Questions Practice"
                ])
            ]),

            // ── 15. Computer Studies / ICT ─────────────────────────────────
            ("Computer Studies", "#0ea5e9", [
                (10, [
                    "Introduction to Computers (History, Generations)",
                    "Computer Hardware (Input, Output, Storage Devices)",
                    "Computer Software (System and Application Software)",
                    "Number Systems (Binary, Octal, Hexadecimal)",
                    "Operating Systems (Functions, Types)",
                    "Word Processing (Microsoft Word)",
                    "Introduction to the Internet and Email",
                    "Computer Safety, Ethics and Security",
                    "Data and Information",
                    "Basic File Management"
                ]),
                (11, [
                    "Spreadsheets (Microsoft Excel, Formulas, Charts)",
                    "Database Management (Microsoft Access)",
                    "Presentation Software (Microsoft PowerPoint)",
                    "Introduction to Programming (Concepts, Flowcharts)",
                    "Programming in BASIC or Python (Fundamentals)",
                    "Computer Networks (LAN, WAN, Internet Protocols)",
                    "Boolean Algebra and Logic Gates",
                    "Data Processing (Batch, Real-Time, Online)",
                    "Desktop Publishing",
                    "Multimedia Concepts"
                ]),
                (12, [
                    "Web Design and Development (HTML, CSS)",
                    "Programming (Loops, Arrays, Functions)",
                    "Database Design and SQL Basics",
                    "System Development Life Cycle (SDLC)",
                    "Cybersecurity and Data Protection",
                    "E-Commerce and Digital Economy",
                    "Artificial Intelligence and Emerging Technologies",
                    "ICT and Society (Impact, Careers)",
                    "Computer Maintenance and Troubleshooting",
                    "WASSCE Past Questions Practice"
                ])
            ]),

            // ── 16. Further Mathematics ────────────────────────────────────
            ("Further Mathematics", "#d946ef", [
                (10, [
                    "Sets and Binary Operations",
                    "Surds and Indices (Advanced)",
                    "Polynomials (Factor and Remainder Theorem)",
                    "Quadratic and Cubic Equations",
                    "Logical Reasoning",
                    "Permutations and Combinations",
                    "Binomial Theorem",
                    "Trigonometric Functions and Identities",
                    "Coordinate Geometry (Advanced)",
                    "Vectors (Two and Three Dimensions)"
                ]),
                (11, [
                    "Complex Numbers",
                    "Matrices (Operations, Inverse, Determinants)",
                    "Linear Transformations",
                    "Differentiation (Limits, Techniques, Applications)",
                    "Integration (Techniques, Definite Integrals)",
                    "Conic Sections (Circle, Parabola, Ellipse, Hyperbola)",
                    "Trigonometry (Compound Angles, Equations)",
                    "Probability Distributions (Binomial, Poisson)",
                    "Statistics (Regression, Correlation)",
                    "Mechanics (Kinematics)"
                ]),
                (12, [
                    "Mechanics (Dynamics, Projectiles)",
                    "Mechanics (Statics, Friction, Equilibrium)",
                    "Differential Equations (First Order)",
                    "Integration (Applications — Area, Volume)",
                    "Numerical Methods (Newton-Raphson, Trapezium Rule)",
                    "Vectors (Scalar and Vector Products)",
                    "Statistics (Normal Distribution, Hypothesis Testing)",
                    "Further Probability",
                    "Mathematical Modelling",
                    "WASSCE Past Questions Practice"
                ])
            ]),

            // ── 17. Horticulture ───────────────────────────────────────────
            ("Horticulture", "#65a30d", [
                (10, [
                    "Introduction to Horticulture (Scope and Importance)",
                    "Classification of Horticultural Crops",
                    "Climate and Soil Requirements for Horticulture",
                    "Plant Propagation (Seeds, Cuttings, Grafting, Budding)",
                    "Nursery Establishment and Management",
                    "Vegetable Production (Common Vegetables)",
                    "Fruit Production (Tropical Fruits)",
                    "Garden Tools and Equipment",
                    "Irrigation and Water Management in Horticulture",
                    "Pest and Disease Management in Gardens"
                ]),
                (11, [
                    "Ornamental Plants and Landscaping",
                    "Greenhouse and Protected Cultivation",
                    "Plant Growth Regulators and Hormones",
                    "Flower Production and Management",
                    "Spice and Medicinal Crop Production",
                    "Vegetable Seed Production",
                    "Fertiliser Use in Horticulture",
                    "Weed Management in Horticultural Crops",
                    "Post-Harvest Handling and Storage",
                    "Marketing of Horticultural Produce"
                ]),
                (12, [
                    "Advanced Propagation Techniques (Tissue Culture)",
                    "Orchard Management and Fruit Tree Care",
                    "Plantation Crop Production (Cashew, Oil Palm, Cocoa)",
                    "Urban and Peri-Urban Horticulture",
                    "Organic Horticulture and Sustainable Practices",
                    "Horticultural Crop Improvement",
                    "Economics of Horticultural Production",
                    "Agribusiness in Horticulture",
                    "Environmental Issues in Horticulture",
                    "WASSCE Past Questions Practice"
                ])
            ]),

            // ── 18. Floriculture ───────────────────────────────────────────
            ("Floriculture", "#e879f9", [
                (10, [
                    "Introduction to Floriculture (Scope and Importance)",
                    "Classification of Flowers and Ornamental Plants",
                    "Flower Anatomy and Physiology",
                    "Climate and Soil Requirements for Flower Production",
                    "Propagation of Flowering Plants",
                    "Nursery Management for Flowers",
                    "Common Cut Flowers (Types, Growing Conditions)",
                    "Potted Plants and Indoor Gardening",
                    "Garden Design Principles",
                    "Tools and Equipment for Floriculture"
                ]),
                (11, [
                    "Cut Flower Production (Rose, Chrysanthemum, Carnation)",
                    "Greenhouse Technology for Flower Production",
                    "Plant Growth Regulators in Floriculture",
                    "Floral Design and Arrangement",
                    "Landscape Design and Installation",
                    "Turf and Lawn Management",
                    "Pest and Disease Management in Flowers",
                    "Post-Harvest Handling of Cut Flowers",
                    "Drying and Preservation of Flowers",
                    "Flower Shows and Exhibitions"
                ]),
                (12, [
                    "Commercial Flower Production and Marketing",
                    "Export and Import of Flowers",
                    "Foliage and Tropical Ornamental Plants",
                    "Floriculture Business Management",
                    "Tissue Culture in Floriculture",
                    "Environmental Impact of Floriculture",
                    "Organic Flower Production",
                    "Urban Greening and Beautification Projects",
                    "Careers in Floriculture and Landscaping",
                    "WASSCE Past Questions Practice"
                ])
            ]),

            // ── 19. Fisheries ──────────────────────────────────────────────
            ("Fisheries", "#0891b2", [
                (10, [
                    "Introduction to Fisheries (Scope, Importance)",
                    "Aquatic Ecosystems (Freshwater, Marine, Brackish)",
                    "Classification of Fish and Aquatic Organisms",
                    "Fish Anatomy and Physiology",
                    "Fish Nutrition and Feeding",
                    "Types of Fishing (Artisanal, Industrial, Recreational)",
                    "Fishing Gear and Methods",
                    "Fisheries in The Gambia (Overview)",
                    "Water Quality and Management",
                    "Introduction to Aquaculture"
                ]),
                (11, [
                    "Aquaculture Systems (Ponds, Cages, Raceways)",
                    "Fish Farming (Species Selection, Stocking)",
                    "Fish Breeding and Hatchery Management",
                    "Feed Formulation for Fish",
                    "Fish Health and Disease Management",
                    "Fisheries Management and Conservation",
                    "Fish Processing and Preservation (Smoking, Drying, Freezing)",
                    "Fisheries Legislation and Regulations",
                    "Ornamental Fish Keeping",
                    "Socio-Economic Importance of Fisheries"
                ]),
                (12, [
                    "Advanced Aquaculture Techniques",
                    "Mariculture (Shrimp, Oyster, Seaweed Farming)",
                    "Fisheries Economics and Marketing",
                    "Fish Quality Control and Food Safety",
                    "Environmental Impact of Fishing and Aquaculture",
                    "Fisheries and Climate Change",
                    "International Fisheries Organisations",
                    "Biotechnology in Fisheries",
                    "Entrepreneurship in Fisheries",
                    "WASSCE Past Questions Practice"
                ])
            ])
        ];
    }
}
