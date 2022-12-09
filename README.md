<h2> 
  Sobre o projeto
</h2>

<p> 
  Esse projeto foi desenvolvido baseado em um teste técnico. 
</p> 

<p>
  É esperado que um funcionário trabalhe de segunda a sexta, e os fins de semana são considerados "dias extras".
</p>

<p> 
  Você escolhe uma pasta com vários CSV com nome no seguinte formato: 
  </br>
  "Nome do Departamento - Mês Vigência - Ano Vigência"
</p>

<p>
  Exemplo:
</p>

```
  Departamento1-Abril-2022.csv
```

<p> 
  Contendo informações de funcionários separados por ' ; ' e ordenados da seguinte forma: 
  </br>
  Código;NomeFuncionário;ValorHora(R$ 00,00);Data(dd-MM-yyyy);HoraEntrada(HH:mm:ss);HoraSaída(HH:mm:ss);Almoço(00:00-00:00)
</p>

<p>
  Exemplo:
</p>

```
  01;Funcionário um;R$ 15,00;10/10/2022;08:00:00;17:00:00;12:00 - 13:00
  02;Funcionário dois;R$ 14,50;10/10/2022;09:00:00;17:00:00;12:00 - 13:00
  01;Funcionário um;R$ 15,00;11/10/2022;08:00:00;18:00:00;12:00 - 13:00
```

<p>
  O output é um json que detalha informações do departamento e de todos funcionários, horas extras feitas por funcionários, horas faltantes, pagamento extra, pagamento total e descontos.
</p>


<p> 
  Exemplo:
</p>

```javascript
[
    {
        "Departamento": "Departamento1",
        "MesVigencia": "Abril",
        "AnoVigencia": 2022,
        "TotalPagar": -424350.0,
        "TotalDescontos": 461450.0,
        "TotalExtras": 1500.0,
        "Funcionarios": [
            {
                "Nome": "Funcionário um",
                "Codigo": 1,
                "TotalReceber": 25500.0,
                "HorasExtras": 1.0,
                "HorasDebito": 0.0,
                "DiasFalta": 19,
                "DiasExtra": 0,
                "DiasTrabalhados": 2
            },
            {
                "Nome": "Funcionário dois",
                "Codigo": 2,
                "TotalReceber": 10150.0,
                "HorasExtras": 0.0,
                "HorasDebito": 1.0,
                "DiasFalta": 20,
                "DiasExtra": 0,
                "DiasTrabalhados": 1
            }
        ]
    }
]
```
