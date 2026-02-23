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
                    Description = $"{subjectName} for Grade {grade}",
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
            ("Mathematics", "#ef4444", [
                (10, [
                    "Algebraic Expressions",
                    "Equations and Inequalities",
                    "Number Patterns",
                    "Functions and Graphs",
                    "Finance and Growth",
                    "Trigonometry",
                    "Analytical Geometry",
                    "Euclidean Geometry",
                    "Statistics",
                    "Probability",
                    "Measurement"
                ]),
                (11, [
                    "Exponents and Surds",
                    "Equations and Inequalities",
                    "Number Patterns and Sequences",
                    "Functions (Parabola, Hyperbola, Exponential)",
                    "Finance, Growth and Decay",
                    "Trigonometry (Identities, Equations, Graphs)",
                    "Analytical Geometry (Circles)",
                    "Euclidean Geometry (Circle Theorems)",
                    "Statistics (Regression, Correlation)",
                    "Probability (Tree Diagrams, Contingency Tables)"
                ]),
                (12, [
                    "Sequences and Series",
                    "Functions and Inverse Functions",
                    "Finance, Growth and Decay (Annuities)",
                    "Differential Calculus",
                    "Trigonometry (Compound and Double Angles)",
                    "Analytical Geometry (Circles, Tangents)",
                    "Euclidean Geometry (Proportionality, Similarity)",
                    "Statistics (Counting Principles)",
                    "Probability (Fundamental Counting Principle)"
                ])
            ]),

            ("English", "#3b82f6", [
                (10, [
                    "Comprehension and Language",
                    "Summary Writing",
                    "Literature: Novel Study",
                    "Literature: Poetry",
                    "Literature: Short Stories",
                    "Literature: Drama",
                    "Creative Writing (Narrative, Descriptive)",
                    "Transactional Writing (Letters, Reports)",
                    "Visual Literacy",
                    "Grammar and Sentence Structure"
                ]),
                (11, [
                    "Comprehension and Language",
                    "Summary Writing",
                    "Literature: Novel Study",
                    "Literature: Poetry Analysis",
                    "Literature: Short Stories Analysis",
                    "Literature: Drama Analysis",
                    "Creative Writing (Essays, Narratives)",
                    "Transactional Writing (Formal Letters, Reviews)",
                    "Visual Literacy and Advertising",
                    "Grammar, Editing and Language Structures"
                ]),
                (12, [
                    "Comprehension and Language",
                    "Summary Writing",
                    "Literature: Novel Study (Exam Prep)",
                    "Literature: Poetry (Exam Prep)",
                    "Literature: Drama (Exam Prep)",
                    "Creative Writing (Argumentative, Reflective)",
                    "Transactional Writing (Speech, Interview)",
                    "Visual Literacy (Cartoons, Advertisements)",
                    "Grammar and Editing",
                    "Exam Preparation and Techniques"
                ])
            ]),

            ("Science", "#22c55e", [
                (10, [
                    "Scientific Method and Skills",
                    "Matter and Classification",
                    "States of Matter and Energy",
                    "The Atom",
                    "Chemical Bonding",
                    "Transverse Waves",
                    "Electromagnetic Radiation",
                    "Magnetism",
                    "Electrostatics",
                    "Electric Circuits"
                ]),
                (11, [
                    "Vectors and Scalars",
                    "Newton's Laws of Motion",
                    "Mechanical Energy",
                    "Waves, Sound and Light",
                    "Chemical Change (Stoichiometry)",
                    "Intermolecular Forces",
                    "Ideal Gases",
                    "Quantitative Aspects of Chemical Change",
                    "Electrostatics (Electric Fields)",
                    "Electric Circuits (Series and Parallel)"
                ]),
                (12, [
                    "Momentum and Impulse",
                    "Vertical Projectile Motion",
                    "Organic Chemistry",
                    "Work, Energy and Power",
                    "Doppler Effect",
                    "Electrodynamics",
                    "Optical Phenomena",
                    "Electrochemistry",
                    "Chemical Equilibrium",
                    "Acids and Bases"
                ])
            ]),

            ("Physics", "#a855f7", [
                (10, [
                    "Units and Measurements",
                    "Motion in One Dimension",
                    "Forces and Newton's Laws",
                    "Gravity and Mechanical Energy",
                    "Transverse Pulses and Waves",
                    "Sound Waves",
                    "Electromagnetic Radiation",
                    "Electrostatics",
                    "Electric Circuits",
                    "Magnetism"
                ]),
                (11, [
                    "Vectors in Two Dimensions",
                    "Newton's Laws (Applications)",
                    "Momentum",
                    "Work, Energy and Power",
                    "Waves (Interference, Diffraction)",
                    "Sound (Doppler Effect intro)",
                    "Light (Refraction, Snell's Law)",
                    "Electrostatics (Coulomb's Law)",
                    "Electric Circuits (Internal Resistance)",
                    "Electromagnetism"
                ]),
                (12, [
                    "Momentum and Impulse (Advanced)",
                    "Vertical Projectile Motion",
                    "Work, Energy and Power (Advanced)",
                    "Doppler Effect",
                    "Electrostatics (Electric Fields)",
                    "Electric Circuits (Advanced)",
                    "Electrodynamics (Motors, Generators)",
                    "Optical Phenomena (Photoelectric Effect)",
                    "Electromagnetic Radiation (Atomic Spectra)",
                    "Nuclear Physics"
                ])
            ]),

            ("Biology", "#ec4899", [
                (10, [
                    "Chemistry of Life",
                    "Cell Structure and Function",
                    "Cell Division (Mitosis)",
                    "Plant and Animal Tissues",
                    "Biodiversity and Classification",
                    "History of Life on Earth",
                    "Biosphere to Ecosystems",
                    "Energy Flow and Nutrient Cycling",
                    "Gaseous Exchange",
                    "Excretion in Humans"
                ]),
                (11, [
                    "Biodiversity and Classification (Detailed)",
                    "History of Life on Earth (Evolution)",
                    "Plant and Animal Tissues (Advanced)",
                    "Support and Transport Systems in Plants",
                    "Support and Transport Systems in Animals",
                    "Biosphere to Ecosystems (Population Ecology)",
                    "Gaseous Exchange (Detailed)",
                    "Excretion in Humans (Urinary System)",
                    "Cell Division (Meiosis)",
                    "Reproduction in Vertebrates"
                ]),
                (12, [
                    "DNA, RNA and Protein Synthesis",
                    "Meiosis and Genetics",
                    "Evolution (Natural Selection, Speciation)",
                    "Human Evolution",
                    "Nervous System and Senses",
                    "Endocrine System",
                    "Homeostasis",
                    "Human Reproduction",
                    "Response of the Immune System",
                    "Human Impact on the Environment"
                ])
            ]),

            ("Chemistry", "#f59e0b", [
                (10, [
                    "Atomic Structure",
                    "Periodic Table",
                    "Chemical Bonding",
                    "Physical and Chemical Change",
                    "Representing Chemical Change",
                    "Magnetism and Electrostatics",
                    "Intermolecular Forces (Intro)",
                    "Solutions and Solubility",
                    "Quantitative Aspects (Mole Concept)",
                    "Energy and Chemical Change"
                ]),
                (11, [
                    "Atomic Combinations (Molecular Structure)",
                    "Intermolecular Forces (Advanced)",
                    "Ideal Gases (Gas Laws)",
                    "Quantitative Aspects (Stoichiometry)",
                    "Energy and Chemical Change (Enthalpy)",
                    "Types of Reactions (Acid-Base, Redox)",
                    "The Lithosphere (Mining, Energy)",
                    "Rate of Reactions",
                    "Chemical Equilibrium (Introduction)",
                    "Electrochemistry (Introduction)"
                ]),
                (12, [
                    "Organic Chemistry (Nomenclature)",
                    "Organic Chemistry (Reactions)",
                    "Organic Chemistry (Polymers, Plastics)",
                    "Rate of Reactions (Factors, Mechanism)",
                    "Chemical Equilibrium (Le Chatelier's)",
                    "Acids and Bases (pH, Titrations)",
                    "Electrochemistry (Galvanic, Electrolytic Cells)",
                    "The Chemical Industry (Fertilizers)",
                    "Quantitative Analysis",
                    "Exam Revision and Problem Solving"
                ])
            ]),

            ("Agriculture", "#84cc16", [
                (10, [
                    "Introduction to Agriculture",
                    "Soil Science (Formation, Types)",
                    "Plant Studies (Structure, Growth)",
                    "Animal Studies (Classification, Nutrition)",
                    "Agricultural Economics (Basic)",
                    "Farm Planning and Layout",
                    "Water Management (Irrigation)",
                    "Pest and Disease Control",
                    "Sustainable Farming Practices",
                    "Agricultural Technology"
                ]),
                (11, [
                    "Soil Science (Fertility, Conservation)",
                    "Plant Production (Crop Management)",
                    "Animal Production (Livestock Management)",
                    "Animal Nutrition and Feeding",
                    "Animal Health and Diseases",
                    "Agricultural Economics (Markets, Pricing)",
                    "Farm Management and Planning",
                    "Mechanization and Technology",
                    "Environmental Impact of Agriculture",
                    "Indigenous Knowledge Systems in Agriculture"
                ]),
                (12, [
                    "Soil Science (Advanced, Land Use)",
                    "Plant Production (Advanced Techniques)",
                    "Animal Production (Breeding, Genetics)",
                    "Animal Reproduction",
                    "Agricultural Economics (Business Plans)",
                    "Farm Management (Labour, Finance)",
                    "Agro-Processing and Beneficiation",
                    "Climate Change and Agriculture",
                    "Food Security and Safety",
                    "Entrepreneurship in Agriculture"
                ])
            ]),

            ("Geography", "#06b6d4", [
                (10, [
                    "The Atmosphere and Weather",
                    "Geomorphology (Surface Processes)",
                    "Map Skills and Techniques",
                    "Population Studies",
                    "Water Resources",
                    "Climate and Weather (South Africa)",
                    "Settlement Geography",
                    "Economic Geography (Primary Activities)",
                    "Environmental Geography",
                    "GIS and Remote Sensing (Introduction)"
                ]),
                (11, [
                    "Climate and Weather (Global Patterns)",
                    "Geomorphology (Drainage Systems)",
                    "Map Work and Calculations",
                    "Population (Migration, Urbanization)",
                    "Economic Geography (Secondary, Tertiary)",
                    "Development Geography",
                    "Resources and Sustainability",
                    "Hazards and Disasters",
                    "GIS Applications",
                    "Tourism and the Environment"
                ]),
                (12, [
                    "Climate and Weather (Synoptic Charts)",
                    "Geomorphology (Fluvial, Coastal Landscapes)",
                    "Map Work (Advanced Calculations)",
                    "Economic Geography of South Africa",
                    "Development Geography (Global Issues)",
                    "Resources and Sustainability (Energy)",
                    "Hazards and Disasters (Risk Management)",
                    "GIS (Advanced Applications)",
                    "Spatial Planning",
                    "Exam Preparation and Map Skills"
                ])
            ])
        ];
    }
}
