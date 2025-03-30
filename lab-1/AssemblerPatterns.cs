using System.Collections.Generic;

namespace AssemblerLexer
{
    public interface IPatternProvider
    {
        IEnumerable<ITokenPattern> GetTokenPatterns();
    }

    public class AssemblerPatternProvider : IPatternProvider
    {
        public IEnumerable<ITokenPattern> GetTokenPatterns()
        {
            var patterns = new List<ITokenPattern>();
            
            foreach (var (pattern, tokenType) in AssemblerPatterns.GetPatterns())
            {
                patterns.Add(new RegexTokenPattern(pattern, tokenType));
            }
            
            return patterns;
        }
    }

    public static class AssemblerPatterns
    {
        public static List<(string Pattern, TokenType TokenType)> GetPatterns()
        {
            // Original pattern definitions (unchanged)
            return new List<(string, TokenType)>
            {
                // Коментарі
                (@";.*", TokenType.COMMENT),

                // Рядкові константи
                (@"\""[^\""]*\""", TokenType.STRING),
                (@"\'[^\']*\'", TokenType.STRING),

                // Директиви
                (@"\.(MODEL|STACK|DATA|CODE|SEGMENT|ENDS|PROC|ENDP|DB|DW|DD|DQ|DT|DF|DP|END|ASSUME|ORG|OFFSET|PTR|TYPE|SIZE|LENGTH|EQU|=|\$)",
                TokenType.DIRECTIVE),

                // Регістри
                (@"\b(AX|BX|CX|DX|AL|BL|CL|DL|AH|BH|CH|DH|SI|DI|SP|BP|CS|DS|ES|SS|FS|GS|" +
                 @"EAX|EBX|ECX|EDX|ESI|EDI|ESP|EBP|CR0|CR2|CR3|DR0|DR1|DR2|DR3|DR6|DR7|" +
                 @"TR6|TR7|ST\([0-7]\)|ST)\b",
                 TokenType.REGISTER),

                // x86 інструкції
                (@"\b(AAA|AAD|AAM|AAS|ADC|ADD|AND|CALL|CBW|CLC|CLD|CLI|CMC|CMP|CMPSB|" +
                 @"CMPSW|CWD|DAA|DAS|DEC|DIV|ESC|HLT|IDIV|IMUL|IN|INC|INT|INTO|IRET|JA|" +
                 @"JAE|JB|JBE|JC|JCXZ|JE|JG|JGE|JL|JLE|JMP|JNA|JNAE|JNB|JNBE|JNC|JNE|" +
                 @"JNG|JNGE|JNL|JNLE|JNO|JNP|JNS|JNZ|JO|JP|JPE|JPO|JS|JZ|LAHF|LDS|LEA|" +
                 @"LES|LODSB|LODSW|LOOP|LOOPE|LOOPNE|LOOPNZ|LOOPZ|MOV|MOVSB|MOVSW|MUL|" +
                 @"NEG|NOP|NOT|OR|OUT|POP|POPF|PUSH|PUSHF|RCL|RCR|RET|RETF|RETN|ROL|ROR|" +
                 @"SAHF|SAL|SAR|SBB|SCASB|SCASW|SHL|SHR|STC|STD|STI|STOSB|STOSW|SUB|TEST|" +
                 @"WAIT|XCHG|XLAT|XOR)\b",
                 TokenType.INSTRUCTION),

                // FPU інструкції
                (@"\b(F2XM1|FABS|FADD|FADDP|FBLD|FBSTP|FCHS|FCLEX|FCOM|FCOMP|FCOMPP|" +
                 @"FDECSTP|FDISI|FDIV|FDIVP|FDIVR|FDIVRP|FENI|FFREE|FIADD|FICOM|FICOMP|" +
                 @"FIDIV|FIDIVR|FILD|FIMUL|FINCSTP|FINIT|FIST|FISTP|FISUB|FISUBR|FLD|" +
                 @"FLD1|FLDCW|FLDENV|FLDL2E|FLDL2T|FLDLG2|FLDLN2|FLDPI|FLDZ|FMUL|FMULP|" +
                 @"FNCLEX|FNDISI|FNENI|FNINIT|FNOP|FNSAVE|FNSTCW|FNSTENV|FNSTSW|FPATAN|" +
                 @"FPREM|FPTAN|FRNDINT|FRSTOR|FSAVE|FSCALE|FSQRT|FST|FSTCW|FSTENV|FSTP|" +
                 @"FSTSW|FSUB|FSUBP|FSUBR|FSUBRP|FTST|FWAIT|FXAM|FXCH|FXTRACT|FYL2X|" +
                 @"FYL2XP1)\b",
                 TokenType.INSTRUCTION),

                // MMX/SSE інструкції
                (@"\b(ADDPD|ADDPS|ADDSD|ADDSS|ANDNPD|ANDNPS|ANDPD|ANDPS|CMPPD|CMPPS|" +
                 @"CMPSD|CMPSS|COMISD|COMISS|CVTDQ2PD|CVTDQ2PS|CVTPD2DQ|CVTPD2PI|" +
                 @"CVTPD2PS|CVTPI2PD|CVTPI2PS|CVTPS2DQ|CVTPS2PD|CVTPS2PI|CVTSD2SI|" +
                 @"CVTSD2SS|CVTSI2SD|CVTSI2SS|CVTSS2SD|CVTSS2SI|CVTTPD2DQ|CVTTPD2PI|" +
                 @"CVTTPS2DQ|CVTTPS2PI|CVTTSD2SI|CVTTSS2SI|DIVPD|DIVPS|DIVSD|DIVSS|" +
                 @"MAXPD|MAXPS|MAXSD|MAXSS|MINPD|MINPS|MINSD|MINSS|MOVAPD|MOVAPS|MOVD|" +
                 @"MOVHPD|MOVHPS|MOVLHPS|MOVLPD|MOVLPS|MOVMSKPD|MOVMSKPS|MOVQ|MOVSD|" +
                 @"MOVSS|MOVUPD|MOVUPS|MULPD|MULPS|MULSD|MULSS|ORPD|ORPS|PACKSSDW|" +
                 @"PACKSSWB|PACKUSWB|PADDB|PADDD|PADDQ|PADDSB|PADDSW|PADDUSB|PADDUSW|" +
                 @"PADDW|PAND|PANDN|PAVGB|PAVGW|PCMPEQB|PCMPEQD|PCMPEQW|PCMPGTB|" +
                 @"PCMPGTD|PCMPGTW|PEXTRW|PINSRW|PMADDWD|PMAXSW|PMAXUB|PMINSW|PMINUB|" +
                 @"PMOVMSKB|PMULHUW|PMULHW|PMULLW|PMULUDQ|POR|PSADBW|PSHUFD|PSHUFHW|" +
                 @"PSHUFLW|PSHUFW|PSLLD|PSLLDQ|PSLLQ|PSLLW|PSRAD|PSRAW|PSRLD|PSRLDQ|" +
                 @"PSRLQ|PSRLW|PSUBB|PSUBD|PSUBQ|PSUBSB|PSUBSW|PSUBUSB|PSUBUSW|PSUBW|" +
                 @"PUNPCKHBW|PUNPCKHDQ|PUNPCKHQDQ|PUNPCKHWD|PUNPCKLBW|PUNPCKLDQ|" +
                 @"PUNPCKLQDQ|PUNPCKLWD|PXOR|RCPPS|RCPSS|RSQRTPS|RSQRTSS|SHUFPD|SHUFPS|" +
                 @"SQRTPD|SQRTPS|SQRTSD|SQRTSS|SUBPD|SUBPS|SUBSD|SUBSS|UCOMISD|UCOMISS|" +
                 @"UNPCKHPD|UNPCKHPS|UNPCKLPD|UNPCKLPS|XORPD|XORPS)\b",
                 TokenType.INSTRUCTION),

                // Мітки
                (@"^[A-Za-z_][A-Za-z0-9_]*:", TokenType.LABEL),

                // Числа з плаваючою комою
                (@"-?\d+\.\d+[Ee][+-]?\d+", TokenType.NUMBER),  // науковий запис (1.23E-4)
                (@"-?\d+\.\d+", TokenType.NUMBER),  // десяткові дроби (1.23)

                // Цілі числа
                (@"\b[0-9][0-9A-Fa-f]*[Hh]\b", TokenType.NUMBER),  // шістнадцяткові з H/h в кінці
                (@"\b[0-9]+[0-9A-Fa-f]*F\b", TokenType.NUMBER),  // шістнадцяткові з F в кінці
                (@"\b0x[0-9A-Fa-f]+\b", TokenType.NUMBER),  // шістнадцяткові (0x...)
                (@"\b[0-9]+d?\b", TokenType.NUMBER),  // десяткові
                (@"\b[01]+b\b", TokenType.NUMBER),  // двійкові
                (@"\b[0-7]+[oOqQ]\b", TokenType.NUMBER),  // вісімкові

                // Оператори
                (@"[\+\-\*\/\[\]\(\),\:,\=]", TokenType.OPERATOR),

                // Спеціальні символи
                (@"[\$\?\@\#\&\|\^\%\!]", TokenType.OPERATOR),

                // Ідентифікатори
                (@"[A-Za-z_][A-Za-z0-9_]*", TokenType.IDENTIFIER),
            };
        }
    }
}