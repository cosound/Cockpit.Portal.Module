using System.Xml.Linq;
using Chaos.Cockpit.Core.Core.Validation;

namespace Chaos.Cockpit.Core.Main
{
  using System.Collections.Generic;
  using Api.Binding;
  using Api.Result;
  using Core;
  using Portal.Core;
  using Portal.Core.Module;

  public class CockpitModule : IModuleConfig
  {
    public void Load(IPortalApplication portalApplication)
    {
      CockpitContext.QuestionnaireGateway = new Data.InMemory.QuestionnaireGateway();
      CockpitContext.QuestionGateway = new Data.InMemory.QuestionGateway();

      var question = new Core.Questionnaire
        {
          Id = "a12f",
          Name = "Sample QuestionnaireResult",
          Slides = new List<Slide>()
            {
              new Slide
                {
                  Questions = new List<Question>()
                    {
                      new Question("Monitor")
                        {
                          Id = "1234",
                          Input =
                            XDocument.Parse(
                              "<Inputs><Events><Event><Type>Start</Type><Id>.\\</Id><Method/><Data/></Event></Events><Contexts><Context><Type/><Data/></Context><Context><Type>IPaddress</Type><Data/></Context></Contexts></Inputs>")
                                     .Root.Elements(),
                          Output = new Output
                            {
                              SimpleValues = new []
                                {
                                  new SimpleValue("Key1", "Value1")
                                }
                            }
                        }
                    }
                },
              new Slide
                {
                  Questions = new List<Question>()
                    {
                      new Question("Freetext")
                        {
                          Id = "12345",
                          Input =
                            XDocument.Parse(
                              "<Inputs><Events><Event><Type>Start,Stop</Type><Id>.\\</Id><Method/><Data/></Event></Events><Instrument><Label>Please enter your Birthplace:</Label></Instrument></Inputs>")
                                     .Root.Elements()
                        },
                      new Question("RadioButtonGroup")
                        {
                          Id = "123456",
                          Input =
                            XDocument.Parse(
                              "<Inputs><Events><Event><Type>Start,Stop</Type><Id>.\\</Id><Method/><Data/></Event></Events><Instrument><HeaderLabel>Gender</HeaderLabel><Items><Item><Label>Male</Label><Selected>0</Selected><Id>M</Id></Item><Item><Label>Female</Label><Selected>0</Selected><Id>F</Id></Item><Item><Label>Other</Label><Selected>0</Selected><Id>O</Id></Item></Items><Stimulus/></Instrument></Inputs>")
                                     .Root.Elements()
                        }
                    }
                },
              new Slide
                {
                  Questions = new List<Question>()
                    {
                      new Question("CheckBoxGroup")
                        {
                          Id = "123456",
                          Input =
                            XDocument.Parse(
                              "<Inputs><Events><Event><Type>Start,Stop</Type><Id>.\\</Id><Method/><Data/></Event></Events><Instrument><HeaderLabel>Select the genres you prefer (0-2)</HeaderLabel><MinNoOfSelections>0</MinNoOfSelections><MaxNoOfSelections>2</MaxNoOfSelections><Items><Item><Label>Rock</Label><Selected>0</Selected><Id>1</Id></Item><Item><Label>Pop</Label><Selected>0</Selected><Id>2</Id></Item><Item><Label>Jazz</Label><Selected>0</Selected><Id>3</Id></Item><Item><Label>Other</Label><Selected>0</Selected><Id>4</Id></Item></Items><Stimulus/></Instrument></Inputs>")
                                     .Root.Elements()
                        }
                    }
                },
              new Slide
                {
                  Questions = new List<Question>()
                    {
                      new Question("CheckBoxGroup")
                        {
                          Id = "RadioButtonGroup0",
                          Input =
                            XDocument.Parse(
                              "<Inputs><Events><Event><Type>Start,Stop</Type><Id>.\\</Id><Method/><Data/></Event></Events><Instrument><HeaderLabel>Select the genres you prefer (0-2)</HeaderLabel><MinNoOfSelections>0</MinNoOfSelections><MaxNoOfSelections>2</MaxNoOfSelections><Items><Item><Label>Indie</Label><Selected>0</Selected><Id>1</Id></Item><Item><Label>Metal</Label><Selected>0</Selected><Id>2</Id></Item></Items><Stimulus/></Instrument></Inputs>")
                                     .Root.Elements()
                        }
                    }
                },
              new Slide
                {
                  Questions = new List<Question>()
                    {
                      new Question("CheckBoxGroup")
                        {
                          Id = "ContinousScale0",
                          Input =
                            XDocument.Parse(
                              "<Inputs><Events><Event><Type>Start,Stop</Type><Id>.\\</Id><Method/><Data/></Event></Events><Instrument><HeaderLabel>Select the genres you prefer (0-2)</HeaderLabel><MinNoOfSelections>0</MinNoOfSelections><MaxNoOfSelections>2</MaxNoOfSelections><Items><Item><Label>KPop</Label><Selected>0</Selected><Id>1</Id></Item><Item><Label>Classical</Label><Selected>0</Selected><Id>2</Id></Item><Item><Label>Latin</Label><Selected>0</Selected><Id>3</Id></Item></Items><Stimulus/></Instrument></Inputs>")
                                     .Root.Elements()
                        }
                    }
                },
              new Slide
                {
                  Questions = new List<Question>()
                    {
                      new Question("OneDScale")
                        {
                          Id = "DropDown0",
                          Input =
                            XDocument.Parse(
                              "<Inputs><Events/><Instrument><HeaderLabel>Rate Kevin Magnussens' performance in 2013 on a scale</HeaderLabel><Position/><X1AxisLabel>Arousal</X1AxisLabel><X2AxisLabel/><Y1AxisLabel>Valance</Y1AxisLabel><Y2AxisLabel/><X1AxisTicks><X1AxisTick><Label>+5</Label><Position>1</Position></X1AxisTick></X1AxisTicks><X2AxisTicks/><Y1AxisTicks/><Y2AxisTicks/><Stimulus/></Instrument></Inputs>")
                                     .Root.Elements()
                        }
                    }
                },
              new Slide
                {
                  Questions = new List<Question>()
                    {
                      new Question("TwoDKScaleDD")
                        {
                          Id = "1234567",
                          Input =
                            XDocument.Parse(
                              "<Inputs><Events><Event><Id>.\\</Id><Type>Start,Stop</Type><Method/><Data/></Event><Event><Id>.\\Instrument\\Items\\Stimulus</Id><Type>Start,Stop</Type><Method/><Data/></Event><Event><Id>.\\Instrument\\Items\\*,.\\Instrument\\Items\\List\\*</Id><Type>Select</Type><Method>*</Method><Data/></Event><Event><Id>.\\Instrument\\Items\\Scale\\*</Id><Type>Change</Type><Method>*</Method><Data/></Event></Events><Instrument><HeaderLabel>Rate the following colors by arousal and valance</HeaderLabel><MinNoOfScalings>2</MinNoOfScalings><X1AxisLabel>Arousal</X1AxisLabel><X2AxisLabel/><Y1AxisLabel>Valance</Y1AxisLabel><Y2AxisLabel/><X1AxisTicks><X1AxisTick><Label>Sad, Annoyed</Label><Position>0</Position></X1AxisTick></X1AxisTicks><X2AxisTicks><X2AxisTick><Label>Happy, Pleased</Label><Position>0</Position></X2AxisTick></X2AxisTicks><Y1AxisTicks><Y1AxisTick><Label>Excited, Aroused</Label><Position>0</Position></Y1AxisTick></Y1AxisTicks><Y2AxisTicks><Y2AxisTick><Label>Passive, Calm</Label><Position>0</Position></Y2AxisTick></Y2AxisTicks><Items><Item><Id>4121</Id><List><Label>Green</Label><Position>1</Position></List><Scale><Label>G</Label><Position/></Scale><Stimulus><Type>image/jpeg</Type><URI>http://www.americourtusa.com/images/mondoten-color-sample-light-green-large.jpg</URI></Stimulus></Item><Item><Id>1321</Id><List><Label>Red</Label><Position>2</Position></List><Scale><Label>R</Label><Position/></Scale><Stimulus><Type>image/jpeg</Type><Label/><URI>http://www.americourtusa.com/images/mondoten-color-sample-california-red.jpg</URI></Stimulus></Item><Item><Id>23431</Id><List><Label>Blue</Label><Position>3</Position></List><Scale><Label>G</Label><Position/></Scale><Stimulus><Type>image/jpeg</Type><Label/><URI>http://www.americourtusa.com/images/mondoten-color-sample-us-open-blue.jpg</URI></Stimulus></Item><Item><Id>433431</Id><List><Label>Purple</Label><Position>4</Position></List><Scale><Label>P</Label><Position/></Scale><Stimulus><Type>image/jpeg</Type><Label/><URI>http://www.americourtusa.com/images/mondoten-color-sample-pro-purple-large.jpg</URI></Stimulus></Item></Items></Instrument></Inputs>")
                                     .Root.Elements()
                        }
                    }
                },
              new Slide
                {
                  Questions = new List<Question>()
                    {
                      new Question("Monitor")
                        {
                          Id = "123456723",
                          Input =
                            XDocument.Parse(
                              "<Inputs><Events><Event><Type>Start</Type><Id>.\\</Id><Method/><Data/></Event></Events><Contexts/></Inputs>")
                                     .Root.Elements()
                        }
                    }
                }
            }
        };
      CockpitContext.QuestionnaireGateway.Set(question);

      portalApplication.AddBinding(typeof (AnswerDto),
                                   new JsonBinding<AnswerDto>());

      portalApplication.MapRoute("/v6/Question",
                                 () => new Api.Endpoints.QuestionExtension(portalApplication));
      portalApplication.MapRoute("/v6/Answer",
                                 () => new Api.Endpoints.AnswerExtension(portalApplication));
    }
  }
}