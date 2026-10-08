using ATBS.Utils;

namespace ATBS.Tests.Utils;

public class ParseHelperShould
{
    [Theory]
    [InlineData("42", 42)]
    [InlineData("  7  ", 7)]
    public void ParseInt_ValidValue_ReturnsParsedInteger(string value, int expected)
    {
        var result = ParseHelper.ParseInt(value, "Age");

        Assert.Equal(expected, result);
    }

    [Fact]
    public void ParseInt_InvalidValue_ThrowsFormatException()
    {
        var exception = Assert.Throws<FormatException>(() => ParseHelper.ParseInt("abc", "Age"));

        Assert.Equal("Age 'abc' is not a valid integer.", exception.Message);
    }

    [Theory]
    [InlineData("12.5", 12.5f)]
    [InlineData("  9.25  ", 9.25f)]
    public void ParseFloat_ValidValue_ReturnsParsedFloat(string value, float expected)
    {
        var result = ParseHelper.ParseFloat(value, "Price");

        Assert.Equal(expected, result);
    }

    [Fact]
    public void ParseFloat_InvalidValue_ThrowsFormatException()
    {
        var exception = Assert.Throws<FormatException>(() => ParseHelper.ParseFloat("nope", "Price"));

        Assert.Equal("Price 'nope' is not a valid number.", exception.Message);
    }

    [Fact]
    public void ParseDate_ValidValue_ReturnsParsedDate()
    {
        var result = ParseHelper.ParseDate("2026-07-20", "DepartureDate");

        Assert.Equal(new DateTime(2026, 7, 20), result.Date);
    }

    [Fact]
    public void ParseDate_InvalidValue_ThrowsFormatException()
    {
        var exception = Assert.Throws<FormatException>(() => ParseHelper.ParseDate("not-a-date", "DepartureDate"));

        Assert.Equal("DepartureDate 'not-a-date' is not a valid date.", exception.Message);
    }
}
