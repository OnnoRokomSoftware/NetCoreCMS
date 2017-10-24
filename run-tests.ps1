Get-ChildItem "NetCoreCMS.UnitTests" | ?{ $_.PsIsContainer } | %{
    pushd "NetCoreCMS.UnitTests\$_"
    & dotnet test
    popd
}