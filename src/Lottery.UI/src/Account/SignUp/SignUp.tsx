import { useForm } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import {
  SignUpRequest,
  SignUpResponse,
  signUpRequestSchema,
} from "./signUp.schema";
import {
  Button,
  InputError,
  PasswordInput,
  Stack,
  TextInput,
  Title,
} from "@mantine/core";
import { useMutation } from "@tanstack/react-query";
import accountService from "../accountService";
import { useNavigate } from "react-router-dom";

interface SignUpProps {}

function SignUp({}: SignUpProps) {
  const {
    register,
    handleSubmit,
    formState: { errors },
  } = useForm<SignUpRequest>({
    resolver: zodResolver(signUpRequestSchema),
  });

  const navigate = useNavigate();

  const mutation = useMutation<SignUpResponse, unknown, SignUpRequest>({
    mutationFn: async (request) => await accountService.signUp(request),
    onSuccess: () => navigate("/home"),
  });

  function FieldError({ name }: { name: keyof SignUpRequest }) {
    const message = errors[name]?.message;
    return message && <InputError>{message}</InputError>;
  }

  return (
    <form onSubmit={handleSubmit((data) => mutation.mutate(data))}>
      <Stack gap={24}>
        <Title>Sign Up</Title>

        <TextInput placeholder="Email" {...register("email")} />
        <FieldError name="email" />

        <TextInput placeholder="Username" {...register("username")} />
        <FieldError name="username" />

        <PasswordInput placeholder="Password" {...register("password")} />
        <FieldError name="password" />

        <Button variant="filled" type="submit">
          Submit
        </Button>
      </Stack>
    </form>
  );
}

export default SignUp;
