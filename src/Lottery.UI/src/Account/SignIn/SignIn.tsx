import { useForm } from "@mantine/form";
import {
  SignInRequest,
  SignInResponse,
  signInRequestSchema,
} from "./signIn.schema";
import { zodResolver } from "mantine-form-zod-resolver";
import { useNavigate } from "react-router-dom";
import { useMutation } from "@tanstack/react-query";
import accountService from "../accountService";
import {
  Anchor,
  Button,
  Checkbox,
  Container,
  Group,
  InputLabel,
  Paper,
  PasswordInput,
  Stack,
  Text,
  TextInput,
  Title,
} from "@mantine/core";
import { useUserStore } from "../../stores/user.store";

interface SignInProps {}

function SignIn({}: SignInProps) {
  const initialValues: SignInRequest = {
    password: "",
    username: "",
    staySignedIn: true,
  };
  const form = useForm<SignInRequest>({
    validate: zodResolver(signInRequestSchema),
    initialValues,
    validateInputOnChange: true,
  });

  const navigate = useNavigate();
  const userStore = useUserStore();

  function onSignInSuccess(response: SignInResponse) {
    userStore.login(response.userType, response.sessionExpiry);
    navigate("/home");
  }

  const mutation = useMutation<SignInResponse, unknown, SignInRequest>({
    mutationFn: async (request) => await accountService.signIn(request),
    onSuccess: onSignInSuccess,
  });

  return (
    <Container p={0} maw={300}>
      <Title>Sign In</Title>
      <Paper shadow="xl" p={24} radius={10}>
        <form onSubmit={form.onSubmit((data) => mutation.mutate(data))}>
          <Stack gap={24}>
            <TextInput
              placeholder="Username"
              {...form.getInputProps("username")}
            />

            <PasswordInput
              placeholder="Password"
              {...form.getInputProps("password")}
            />

            <Group>
              <Checkbox
                {...form.getInputProps("staySignedIn")}
                defaultChecked={initialValues.staySignedIn}
              />
              <InputLabel>Stay signed in</InputLabel>
            </Group>

            <Button variant="filled" type="submit">
              Submit
            </Button>
            <Text size="sm">
              Don't have an account?{" "}
              <Anchor href="/account/signUp">Sign up</Anchor>
            </Text>
          </Stack>
        </form>
      </Paper>
    </Container>
  );
}

export default SignIn;
