using Hl7.Fhir.Model;
using Hl7.Fhir.Utility;
using System;
using System.Collections.Generic;

namespace TaskForQuestionnaireExample
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Generating XML and Json example");
            GenerateTaskForQuestionnaireExample();
        }

        private static void GenerateTaskForQuestionnaireExample()
        {
            string name = "Taks-example";
            DateTimeOffset reminder = DateTimeOffset.UtcNow.AddDays(12);
            DateTimeOffset deadline = DateTimeOffset.UtcNow.AddDays(14);
            DateTimeOffset validTo = DateTimeOffset.UtcNow.AddDays(18).Date;
            Task task = new Task
            {
                Meta = new Meta
                {
                    LastUpdated = DateTimeOffset.UtcNow,
                    Profile = new[] { "http://helsenorge.no/fhir/StructureDefinition/hn-task" }
                },
                //Refeance to the questionnaire  
                Focus = new ResourceReference
                {
                    Reference = "Consent/8482BD5F-DA02-433E-8D43-F76E36A29D3A" 
                },
                Status = Task.TaskStatus.Requested,
                Intent = Task.TaskIntent.Plan,
                Priority = RequestPriority.Routine,
                AuthoredOnElement = new FhirDateTime(DateTimeOffset.Now),
                LastModifiedElement = new FhirDateTime(DateTimeOffset.Now),
                Owner = new ResourceReference
                {
                    Type = ResourceType.Patient.GetLiteral(),
                    Identifier = new Identifier
                    {
                        System = "2.16.578.1.12.4.1.4.1",   // Fødselsnummer
                        Value = "13116900216"               // Line Danser
                    },
                    Display = "Line Danser"
                },
                Requester = new ResourceReference
                {
                    Type = ResourceType.Organization.GetLiteral(),
                    Identifier = new Identifier
                    {
                        System = "2.16.578.1.12.4.1.2.101", // Organisasjonsnummer
                        Value = "948554062"                 // Studentsamskipnaden i Oslo og Akershus
                    },
                    Display = "Studentsamskipnaden i Oslo og Akershus"
                },
                Description = "Vær vennlig å fyll ut vedlagte skjema.",
                Restriction = new Task.RestrictionComponent
                {
                    // ValidTo / Gyldig til
                    Period = new Period
                    {
                        EndElement = new FhirDateTime(DateTimeOffset.Now.AddDays(21))
                    },
                    Recipient = new List<ResourceReference>
                    {
                        new ResourceReference
                        {
                            Extension = new List<Extension>
                            {
                                // Endpoint
                                new Extension
                                {
                                    Url = "http://ehelse.no/fhir/StructureDefinition/sdf-endpoint",
                                    Value = new ResourceReference
                                    {
                                        Reference = "http://skjemakatalog.ehelse.no/api/v1/Endpoint/<id>",
                                        Display = "SiO skjemamottak"
                                    }
                                }
                            },
                            Type = ResourceType.Organization.GetLiteral(),
                            Identifier = new Identifier
                            {
                                System = "2.16.578.1.12.4.1.2.101", // Organisasjonsnummer
                                Value = "948554062"                 // Studentsamskipnaden i Oslo og Akershus
                            },
                            Display = "Studentsamskipnaden i Oslo og Akershus"
                        }
                    }
                },
                Input = new List<Task.ParameterComponent>
                {
                    // Påminnelse (Reminder)
                    new Task.ParameterComponent
                    {
                        Type = new CodeableConcept
                        {
                            Coding = new List<Coding>
                            {
                                new Coding
                                {
                                    System = "http://helsenorge.no/fhir/task-input",
                                    Code = "reminder",
                                    Display = "The reminder for the task"
                                }
                            }
                        },
                        Value = new Date(reminder.Year, reminder.Month, reminder.Day)
                    },
                    // Frist på oppgave (Deadline)
                    new Task.ParameterComponent
                    {
                        Type = new CodeableConcept
                        {
                            Coding = new List<Coding>
                            {
                                new Coding
                                {
                                    System = "http://helsenorge.no/fhir/task-input",
                                    Code = "deadline",
                                    Display = "The deadline for the task"
                                }
                            },
                        },
                        Value = new Date(deadline.Year, deadline.Month, deadline.Day)
                    },
                    // Contact
                    new Task.ParameterComponent
                    {
                        Type = new CodeableConcept
                        {
                            Coding = new List<Coding>
                            {
                                new Coding
                                {
                                    System = "http://helsenorge.no/fhir/task-input",
                                    Code = "contact",
                                    Display = "Contact details for the task"
                                }
                            }
                        },
                        Value = new ContactDetail
                        {
                            Extension = new List<Extension>
                            {
                                new Extension
                                {
                                    Url = "http://helsenorge.no/fhir/StructureDefinition/hn-description",
                                    Value = new FhirString("Ved uklarheter med skjemaet eller annet kontakt SiO.")
                                }
                            },
                            Name = "SiO",
                            Telecom = new List<ContactPoint>
                            {
                                new ContactPoint
                                {
                                    System = ContactPoint.ContactPointSystem.Phone,
                                    Value = "+4722853200"
                                }
                            }
                        }
                    },
                    // Meldingstittel
                    new Task.ParameterComponent
                    {
                        Type = new CodeableConcept
                        {
                            Coding = new List<Coding>
                            {
                                new Coding
                                {
                                    System = "http://helsenorge.no/fhir/task-input",
                                    Code = "title",
                                    Display = "Task title"
                                }
                            }
                        },
                        Value = new FhirString("[Meldingstittel] Skjema <something> fra SIO")
                    },
                    // Name (in addition to title, can be the (technical) name of the Questionnaire)
                    new Task.ParameterComponent
                    {
                        Type = new CodeableConcept
                        {
                            Coding = new List<Coding>
                            {
                                new Coding
                                {
                                    System = "http://helsenorge.no/fhir/task-input",
                                    Code = "name",
                                    Display = "Task name"
                                }
                            }
                        },
                        Value = new FhirString("SIO973")
                    },
                    // DescriptionAboutReceivers
                    new Task.ParameterComponent
                    {
                        Type = new CodeableConcept
                        {
                            Coding = new List<Coding>
                            {
                                new Coding
                                {
                                    System = "http://helsenorge.no/fhir/task-input",
                                    Code = "describe-receiver",
                                    Display = "Description about the receiver(s) and their purpose"
                                }
                            }
                        },
                        Value = new FhirString("This parameter describes the receivers of the task output and their purpose.")
                    },
                    // CanBePerformedBy
                    new Task.ParameterComponent
                    {
                        Type = new CodeableConcept
                        {
                            Coding = new List<Coding>
                            {
                                new Coding
                                {
                                    System = "http://helsenorge.no/fhir/task-input",
                                    Code = "performed-by",
                                    Display = "Who can perform the task"
                                }
                            }
                        },
                        Value = new Coding
                        {
                            System = "http://ehelse.no/fhir/ValueSet/CanBePerformedBy",
                            Code = "1",
                            Display = "Default behavior"
                        }
                    },
                    // AccessibilityToResponse
                    new Task.ParameterComponent
                    {
                        Type = new CodeableConcept
                        {
                            Coding = new List<Coding>
                            {
                                new Coding
                                {
                                    System = "http://helsenorge.no/fhir/task-input",
                                    Code = "access-to-output",
                                    Display = "Who can access the output of the task"
                                }
                            }
                        },
                        Value = new Coding
                        {
                            System = "http://ehelse.no/fhir/ValueSet/AccessibilityToResponse",
                            Code = "1",
                            Display = "Default behavior"
                        }
                    },
                    // SupportsRepresentation
                    new Task.ParameterComponent
                    {
                        Type = new CodeableConcept
                        {
                            Coding = new List<Coding>
                            {
                                new Coding
                                {
                                    System = "http://helsenorge.no/fhir/task-input",
                                    Code = "supports-representation",
                                    Display = "Does the tool used to perform the task support user representation"
                                }
                            }
                        },
                        Value = new FhirBoolean(true)
                    },
                    new Task.ParameterComponent
                    {
                        Type = new CodeableConcept
                        {
                            Coding = new List<Coding>
                            {
                                new Coding
                                {
                                    System = "http://helsenorge.no/fhir/task-input",
                                    Code = "use-context",
                                    Display = "Explains the use context of the Task being pointed at, this is used internally by Helsenorge to map to a consent area"
                                }
                            }
                        },
                        Value = new CodeableConcept
                        {
                            Coding = new List<Coding>
                            {
                                new Coding
                                {
                                    System = "urn:oid:2.16.578.1.12.4.1.1.8655",
                                    Code = "S0305",
                                    Display = "Infeksjonsmedisin"
                                }
                            }
                        }
                    }
                }
            };


            task.SerializeResourceToDiskAsJson($"{name}.json", true);
            task.SerializeResourceToDiskAsXml($"{name}.xml", true);
        }
    }
}
